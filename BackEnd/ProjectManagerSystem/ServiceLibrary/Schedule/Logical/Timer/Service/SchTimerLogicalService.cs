using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnSMTP.Config;
using CommonLibrary.CmnSMTP.Format;
using CommonLibrary.CmnSMTP.Service;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using DataModelLibrary.GlobalSystem.Config;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Project.Format;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Format;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;

namespace ServiceLibrary.Schedule.Logical.Timer.Service;

/// <summary>排程-邏輯-計時器-服務</summary>
public class SchTimerLogicalService : ISchTimerLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<SchTimerLogicalService> _logger;

    #region Common Library

    /// <summary>通用-SMTP-服務</summary>
    private readonly ICmnSmtpService _cmnSmtp;

    #endregion

    #region GlobalSystem

    /// <summary>全域-ApiAppSetting-設定</summary>
    private readonly GsApiAppSettingConfig _gsApiAppSetting;

    #endregion

    #region Core Manager

    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpProjectDbService _coEmpProjectDb;

    /// <summary>核心-員工-專案成員-資料庫服務介面</summary>
    private readonly ICoEmpProjectMemberDbService _coEmpProjectMemberDb;

    /// <summary>核心-員工-專案里程碑-資料庫服務</summary>
    private readonly ICoEmpProjectStoneDbService _coEmpProjectStoneDb;

    /// <summary>核心-員工-專案里程碑工項-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobDbService _coEmpProjectStoneJobDb;

    #endregion

    /// <summary>建構式</summary>
    public SchTimerLogicalService(
        ILogger<SchTimerLogicalService> logger,
        // Common Library
        ICmnSmtpService cmnSmtp,
        // GlobalSystem
        GsApiAppSettingConfig gsApiAppSetting,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpProjectDbService coEmpProjectDb,
        ICoEmpProjectMemberDbService coEmpProjectMemberDb,
        ICoEmpProjectStoneDbService coEmpProjectStoneDb,
        ICoEmpProjectStoneJobDbService coEmpProjectStoneJobDb)
    {
        this._logger = logger;
        // Common Library
        this._cmnSmtp = cmnSmtp;
        // GlobalSystem
        this._gsApiAppSetting = gsApiAppSetting;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpProjectDb = coEmpProjectDb;
        this._coEmpProjectMemberDb = coEmpProjectMemberDb;
        this._coEmpProjectStoneDb = coEmpProjectStoneDb;
        this._coEmpProjectStoneJobDb = coEmpProjectStoneJobDb;
    }

    /// <summary>更新專案狀態</summary>
    public async Task UpdateProjectStatusAsync()
    {
        // db, 核心-員工-專案-資料庫-取得多筆ID從[排程-計時器]
        var coEmpPrjDbGetManyIdFromSchTmrReqObj = new CoEmpPrjDbGetManyIdFromSchTmrReqMdl()
        {
            AtomEmployeeProjectStatusList = new List<DbAtomEmployeeProjectStatusEnum>()
            {
                DbAtomEmployeeProjectStatusEnum.NotStarted,
                DbAtomEmployeeProjectStatusEnum.OnSchedule,
                DbAtomEmployeeProjectStatusEnum.AtRisk,
            },
        };
        var coEmpPrjDbGetManyIdFromSchTmrRspObj = await this._coEmpProjectDb.GetManyIdFromSchTmrAsync(coEmpPrjDbGetManyIdFromSchTmrReqObj);
        if (coEmpPrjDbGetManyIdFromSchTmrRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // db, 核心-員工-專案里程碑-取得多筆
        var coEmpPsDbGetManyReqObj = new CoEmpPsDbGetManyReqMdl()
        {
            EmployeeProjectIdList = coEmpPrjDbGetManyIdFromSchTmrRspObj.EmployeeProjectList
                .Select(x => x.EmployeeProjectID)
                .Distinct()
                .ToList(),
        };
        var coEmpPsDbGetManyRspObj = await this._coEmpProjectStoneDb.GetManyAsync(coEmpPsDbGetManyReqObj);
        if (coEmpPsDbGetManyRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // db, 核心-員工-專案里程碑工項-取得多筆
        var coEmpPsjDbGetManyReqObj = new CoEmpPsjDbGetManyReqMdl()
        {
            EmployeeProjectIdList = coEmpPrjDbGetManyIdFromSchTmrRspObj.EmployeeProjectList
                .Select(x => x.EmployeeProjectID)
                .Distinct()
                .ToList(),
        };
        var coEmpPsjDbGetManyRspObj = await this._coEmpProjectStoneJobDb.GetManyAsync(coEmpPsjDbGetManyReqObj);
        if (coEmpPsjDbGetManyRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 分類各種狀態
        var onScheduleTaskList = new List<CoEmpPsjDbGetManyRspItemJobMdl>();
        var atRiskTaskList = new List<CoEmpPsjDbGetManyRspItemJobMdl>();
        var delayedTaskList = new List<CoEmpPsjDbGetManyRspItemJobMdl>();
        foreach (var employeeProjectStoneTaskItem in coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList)
        {
            // 取得里程碑
            var empProjectStone = coEmpPsDbGetManyRspObj.EmployeeProjectStoneList
                .SingleOrDefault(x => x.EmployeeProjectStoneID == employeeProjectStoneTaskItem.EmployeeProjectStoneID);

            // 延遲: 里程碑結束時間 <= now
            if (empProjectStone.EmployeeProjectStoneEndTime <= DateTimeOffset.UtcNow)
            {
                delayedTaskList.Add(new CoEmpPsjDbGetManyRspItemJobMdl()
                {
                    EmployeeProjectStoneJobID = employeeProjectStoneTaskItem.EmployeeProjectStoneJobID,
                    EmployeeProjectID = employeeProjectStoneTaskItem.EmployeeProjectID,
                    EmployeeProjectStoneID = employeeProjectStoneTaskItem.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobName = employeeProjectStoneTaskItem.EmployeeProjectStoneJobName,
                    EmployeeProjectStoneJobStartTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = employeeProjectStoneTaskItem.EmployeeProjectStoneJobWorkHour,
                    AtomEmployeeProjectStatus = employeeProjectStoneTaskItem.AtomEmployeeProjectStatus,
                    EmployeeProjectStoneJobRemark = employeeProjectStoneTaskItem.EmployeeProjectStoneJobRemark,
                });
                continue;
            }

            // 注意: 工項結束時間 <= now < 里程碑結束時間
            if (employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime <= DateTimeOffset.UtcNow
                && DateTimeOffset.UtcNow < empProjectStone.EmployeeProjectStoneEndTime)
            {
                atRiskTaskList.Add(new CoEmpPsjDbGetManyRspItemJobMdl()
                {
                    EmployeeProjectStoneJobID = employeeProjectStoneTaskItem.EmployeeProjectStoneJobID,
                    EmployeeProjectID = employeeProjectStoneTaskItem.EmployeeProjectID,
                    EmployeeProjectStoneID = employeeProjectStoneTaskItem.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobName = employeeProjectStoneTaskItem.EmployeeProjectStoneJobName,
                    EmployeeProjectStoneJobStartTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = employeeProjectStoneTaskItem.EmployeeProjectStoneJobWorkHour,
                    AtomEmployeeProjectStatus = employeeProjectStoneTaskItem.AtomEmployeeProjectStatus,
                    EmployeeProjectStoneJobRemark = employeeProjectStoneTaskItem.EmployeeProjectStoneJobRemark,
                });
                continue;
            }

            // 如期: 工項開始時間 < now < 工項結束時間
            if (employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime < DateTimeOffset.UtcNow
                && DateTimeOffset.UtcNow < employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime)
            {
                onScheduleTaskList.Add(new CoEmpPsjDbGetManyRspItemJobMdl()
                {
                    EmployeeProjectStoneJobID = employeeProjectStoneTaskItem.EmployeeProjectStoneJobID,
                    EmployeeProjectID = employeeProjectStoneTaskItem.EmployeeProjectID,
                    EmployeeProjectStoneID = employeeProjectStoneTaskItem.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobName = employeeProjectStoneTaskItem.EmployeeProjectStoneJobName,
                    EmployeeProjectStoneJobStartTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = employeeProjectStoneTaskItem.EmployeeProjectStoneJobWorkHour,
                    AtomEmployeeProjectStatus = employeeProjectStoneTaskItem.AtomEmployeeProjectStatus,
                    EmployeeProjectStoneJobRemark = employeeProjectStoneTaskItem.EmployeeProjectStoneJobRemark,
                });
                continue;
            }

            this._logger.LogError($"item: {JsonSerializer.Serialize(employeeProjectStoneTaskItem)}");
        }

        #region 更新工項

        // 更新工項狀態 => 如期
        if (onScheduleTaskList.Any())
        {
            // db, 核心-員工-專案里程碑工項-資料庫-更新多筆
            var coEmpPsjDbUpdateManyReqObj = new CoEmpPsjDbUpdateManyReqMdl()
            {
                EmployeeProjectStoneJobIdList = onScheduleTaskList
                    .Select(x => x.EmployeeProjectStoneJobID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.OnSchedule,
            };
            var coEmpPsjDbUpdateManyRspObj = await this._coEmpProjectStoneJobDb.UpdateManyAsync(coEmpPsjDbUpdateManyReqObj);
            if (coEmpPsjDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // 更新工項狀態 => 注意
        if (atRiskTaskList.Any())
        {
            // db, 核心-員工-專案里程碑工項-資料庫-更新多筆
            var coEmpPsjDbUpdateManyReqObj = new CoEmpPsjDbUpdateManyReqMdl()
            {
                EmployeeProjectStoneJobIdList = atRiskTaskList
                    .Select(x => x.EmployeeProjectStoneJobID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.AtRisk,
            };
            var coEmpPsjDbUpdateManyRspObj = await this._coEmpProjectStoneJobDb.UpdateManyAsync(coEmpPsjDbUpdateManyReqObj);
            if (coEmpPsjDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // 更新工項狀態 => 延遲
        if (delayedTaskList.Any())
        {
            // db, 核心-員工-專案里程碑工項-資料庫-更新多筆
            var coEmpPsjDbUpdateManyReqObj = new CoEmpPsjDbUpdateManyReqMdl()
            {
                EmployeeProjectStoneJobIdList = delayedTaskList
                    .Select(x => x.EmployeeProjectStoneJobID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.Delayed,
            };
            var coEmpPsjDbUpdateManyRspObj = await this._coEmpProjectStoneJobDb.UpdateManyAsync(coEmpPsjDbUpdateManyReqObj);
            if (coEmpPsjDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        #endregion

        #region 更新里程碑

        // 更新里程碑狀態 => 如期
        if (onScheduleTaskList.Any())
        {
            // db, 核心-員工-專案里程碑-資料庫-更新多筆
            var coEmpPsDbUpdateManyReqObj = new CoEmpPsDbUpdateManyReqMdl()
            {
                EmployeeProjectStoneIdList = onScheduleTaskList
                    .Select(x => x.EmployeeProjectStoneID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.OnSchedule,
            };
            var coEmpPsDbUpdateManyRspObj = await this._coEmpProjectStoneDb.UpdateManyAsync(coEmpPsDbUpdateManyReqObj);
            if (coEmpPsDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // 更新里程碑狀態 => 注意
        if (atRiskTaskList.Any())
        {
            // db, 核心-員工-專案里程碑-資料庫-更新多筆
            var coEmpPsDbUpdateManyReqObj = new CoEmpPsDbUpdateManyReqMdl()
            {
                EmployeeProjectStoneIdList = atRiskTaskList
                    .Select(x => x.EmployeeProjectStoneID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.AtRisk,
            };
            var coEmpPsDbUpdateManyRspObj = await this._coEmpProjectStoneDb.UpdateManyAsync(coEmpPsDbUpdateManyReqObj);
            if (coEmpPsDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // 更新里程碑狀態 => 延遲
        if (delayedTaskList.Any())
        {
            // db, 核心-員工-專案里程碑-資料庫-更新多筆
            var coEmpPsDbUpdateManyReqObj = new CoEmpPsDbUpdateManyReqMdl()
            {
                EmployeeProjectStoneIdList = delayedTaskList
                    .Select(x => x.EmployeeProjectStoneID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.Delayed,
            };
            var coEmpPsDbUpdateManyRspObj = await this._coEmpProjectStoneDb.UpdateManyAsync(coEmpPsDbUpdateManyReqObj);
            if (coEmpPsDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        #endregion

        #region 更新專案

        // 更新專案狀態 => 如期
        if (onScheduleTaskList.Any())
        {
            // db, 核心-員工-專案-資料庫-更新多筆
            var coEmpPrjDbUpdateManyReqObj = new CoEmpPrjDbUpdateManyReqMdl()
            {
                EmployeeProjectIdList = onScheduleTaskList
                    .Select(x => x.EmployeeProjectID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.OnSchedule,
            };
            var coEmpPrjDbUpdateManyRspObj = await this._coEmpProjectDb.UpdateManyAsync(coEmpPrjDbUpdateManyReqObj);
            if (coEmpPrjDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // 更新專案狀態 => 注意
        if (atRiskTaskList.Any())
        {
            // db, 核心-員工-專案-資料庫-更新多筆
            var coEmpPrjDbUpdateManyReqObj = new CoEmpPrjDbUpdateManyReqMdl()
            {
                EmployeeProjectIdList = atRiskTaskList
                    .Select(x => x.EmployeeProjectID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.AtRisk,
            };
            var coEmpPrjDbUpdateManyRspObj = await this._coEmpProjectDb.UpdateManyAsync(coEmpPrjDbUpdateManyReqObj);
            if (coEmpPrjDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // 更新專案狀態 => 延遲
        if (delayedTaskList.Any())
        {
            // db, 核心-員工-專案-資料庫-更新多筆
            var coEmpPrjDbUpdateManyReqObj = new CoEmpPrjDbUpdateManyReqMdl()
            {
                EmployeeProjectIdList = delayedTaskList
                    .Select(x => x.EmployeeProjectID)
                    .Distinct()
                    .ToList(),
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.Delayed,
            };
            var coEmpPrjDbUpdateManyRspObj = await this._coEmpProjectDb.UpdateManyAsync(coEmpPrjDbUpdateManyReqObj);
            if (coEmpPrjDbUpdateManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        #endregion


    }

    /// <summary>通知異常專案狀態</summary>
    public async Task NotifyAbnormalProjectStatusAsync()
    {
        /**
         * 從 DB 拉出，延遲，注意，未指派的專案
         * 發出通知給專案成員
         */

        #region 未指派

        // 取得 未指派 的 專案ID
        // db, 核心-員工-專案-取得多筆ID
        var notAssignedCoEmpPrjDbGetManyIdReqObj = new CoEmpPrjDbGetManyIdReqMdl()
        {
            AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.NotAssigned,
        };
        var notAssignedCoEmpPrjDbGetManyIdRspObj = await this._coEmpProjectDb.GetManyIdAsync(notAssignedCoEmpPrjDbGetManyIdReqObj);
        if (notAssignedCoEmpPrjDbGetManyIdRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否有 未指派 的專案
        if (notAssignedCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList.Any() == false)
        {
            // 有 未指派 的專案

            // 取得 未指派 的 員工ID
            // db, 核心-員工-專案成員-取得多筆員工ID
            var notAssignedCoEmpPmDbGetManyEmployeeIdReqObj = new CoEmpPmDbGetManyEmployeeIdReqMdl()
            {
                EmployeeProjectIdList = notAssignedCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList
                    .Select(x => x.EmployeeProjectID)
                    .Distinct()
                    .ToList(),
            };
            var notAssignedCoEmpPmDbGetManyEmployeeIdRspObj = await this._coEmpProjectMemberDb.GetManyEmployeeIdAsync(notAssignedCoEmpPmDbGetManyEmployeeIdReqObj);
            if (notAssignedCoEmpPmDbGetManyEmployeeIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // 取得 未指派 的 員工信箱
            // db, 核心-員工-資訊-資料庫-取得多筆[信箱]
            var notAssignedCoEmpInfoDbGetManyEmailReqObj = new CoEmpInfDbGetManyEmailReqMdl()
            {
                EmployeeIdList = notAssignedCoEmpPmDbGetManyEmployeeIdRspObj.EmployeeProjectMemberList
                    .Select(x => x.EmployeeID)
                    .Distinct()
                    .ToList(),
            };
            var notAssignedCoEmpInfoDbGetManyEmailRspObj = await this._coEmpInfoDb.GetManyEmailAsync(notAssignedCoEmpInfoDbGetManyEmailReqObj);
            if (notAssignedCoEmpInfoDbGetManyEmailRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // 組成 未指派 通知內容
            var notAssignedSb = new StringBuilder();
            notAssignedSb.AppendLine("以下是未指派的專案清單:<br/>");
            foreach (var employeeProjectItem in notAssignedCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList)
            {
                notAssignedSb.AppendLine($"專案網址: {this._gsApiAppSetting.PlatformConfig.ClientFrontSite}/work/project/detail/{employeeProjectItem.EmployeeProjectID}<br/>");
            }

            // 發送 未指派 通知
            var notAssignedCmnSmtpSendReqObj = new CmnSmtpSendReqMdl()
            {
                Config = new CmnSmtpConfig
                {
                    Host = this._gsApiAppSetting.SmtpConfig.Host,
                    Port = this._gsApiAppSetting.SmtpConfig.Port,
                    EnableSsl = this._gsApiAppSetting.SmtpConfig.EnableSsl,
                    SenderEmail = this._gsApiAppSetting.SmtpConfig.FromEmail,
                    SenderName = this._gsApiAppSetting.SmtpConfig.FromName,
                    Username = this._gsApiAppSetting.SmtpConfig.Username,
                    Password = this._gsApiAppSetting.SmtpConfig.Password
                },
                ReceiverList = notAssignedCoEmpInfoDbGetManyEmailRspObj.EmployeeList
                    .Select(x => x.EmployeeEmail)
                    .Distinct()
                    .ToList(),
                Subject = "有尚未指派的專案",
                Body = notAssignedSb.ToString(),
                IsHtml = true,
            };
            var notAssignedCmnSmtpSendRspObj = await this._cmnSmtp.SendAsync(notAssignedCmnSmtpSendReqObj);

        }

        #endregion

        #region 延遲

        // 取得 延遲 的 專案ID
        // db, 核心-員工-專案-取得多筆ID
        var delayedCoEmpPrjDbGetManyIdReqObj = new CoEmpPrjDbGetManyIdReqMdl()
        {
            AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.Delayed,
        };
        var delayedCoEmpPrjDbGetManyIdRspObj = await this._coEmpProjectDb.GetManyIdAsync(delayedCoEmpPrjDbGetManyIdReqObj);
        if (delayedCoEmpPrjDbGetManyIdRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否有 延遲 的專案
        if (delayedCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList.Any() == false)
        {
            // 有 延遲 的專案

            // 取得 延遲 的 員工ID
            // db, 核心-員工-專案成員-取得多筆員工ID
            var delayedCoEmpPmDbGetManyEmployeeIdReqObj = new CoEmpPmDbGetManyEmployeeIdReqMdl()
            {
                EmployeeProjectIdList = delayedCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList
                    .Select(x => x.EmployeeProjectID)
                    .Distinct()
                    .ToList(),
            };
            var delayedCoEmpPmDbGetManyEmployeeIdRspObj = await this._coEmpProjectMemberDb.GetManyEmployeeIdAsync(delayedCoEmpPmDbGetManyEmployeeIdReqObj);
            if (delayedCoEmpPmDbGetManyEmployeeIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // 取得 延遲 的 員工信箱
            // db, 核心-員工-資訊-資料庫-取得多筆[信箱]
            var delayedCoEmpInfoDbGetManyEmailReqObj = new CoEmpInfDbGetManyEmailReqMdl()
            {
                EmployeeIdList = delayedCoEmpPmDbGetManyEmployeeIdRspObj.EmployeeProjectMemberList
                    .Select(x => x.EmployeeID)
                    .Distinct()
                    .ToList(),
            };
            var delayedCoEmpInfoDbGetManyEmailRspObj = await this._coEmpInfoDb.GetManyEmailAsync(delayedCoEmpInfoDbGetManyEmailReqObj);
            if (delayedCoEmpInfoDbGetManyEmailRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // 組成 延遲 通知內容
            var delayedSb = new StringBuilder();
            delayedSb.AppendLine("以下是延遲的專案清單:<br/>");
            foreach (var employeeProjectItem in delayedCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList)
            {
                delayedSb.AppendLine($"專案網址: {this._gsApiAppSetting.PlatformConfig.ClientFrontSite}/work/project/detail/{employeeProjectItem.EmployeeProjectID}<br/>");
            }

            // 發送 延遲 通知
            var delayedCmnSmtpSendReqObj = new CmnSmtpSendReqMdl()
            {
                Config = new CmnSmtpConfig
                {
                    Host = this._gsApiAppSetting.SmtpConfig.Host,
                    Port = this._gsApiAppSetting.SmtpConfig.Port,
                    EnableSsl = this._gsApiAppSetting.SmtpConfig.EnableSsl,
                    SenderEmail = this._gsApiAppSetting.SmtpConfig.FromEmail,
                    SenderName = this._gsApiAppSetting.SmtpConfig.FromName,
                    Username = this._gsApiAppSetting.SmtpConfig.Username,
                    Password = this._gsApiAppSetting.SmtpConfig.Password
                },
                ReceiverList = delayedCoEmpInfoDbGetManyEmailRspObj.EmployeeList
                    .Select(x => x.EmployeeEmail)
                    .Distinct()
                    .ToList(),
                Subject = "有延遲的專案",
                Body = delayedSb.ToString(),
                IsHtml = true,
            };
            var delayedCmnSmtpSendRspObj = await this._cmnSmtp.SendAsync(delayedCmnSmtpSendReqObj);

        }


        #endregion

        #region 注意

        // 取得 注意 的 專案ID
        // db, 核心-員工-專案-取得多筆ID
        var atRiskCoEmpPrjDbGetManyIdReqObj = new CoEmpPrjDbGetManyIdReqMdl()
        {
            AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.AtRisk,
        };
        var atRiskCoEmpPrjDbGetManyIdRspObj = await this._coEmpProjectDb.GetManyIdAsync(atRiskCoEmpPrjDbGetManyIdReqObj);
        if (atRiskCoEmpPrjDbGetManyIdRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否有 注意 的專案
        if (atRiskCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList.Any() == false)
        {
            // 有 注意 的專案

            // 取得 注意 的 員工ID
            // db, 核心-員工-專案成員-取得多筆員工ID
            var atRiskCoEmpPmDbGetManyEmployeeIdReqObj = new CoEmpPmDbGetManyEmployeeIdReqMdl()
            {
                EmployeeProjectIdList = atRiskCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList
                    .Select(x => x.EmployeeProjectID)
                    .Distinct()
                    .ToList(),
            };
            var atRiskCoEmpPmDbGetManyEmployeeIdRspObj = await this._coEmpProjectMemberDb.GetManyEmployeeIdAsync(atRiskCoEmpPmDbGetManyEmployeeIdReqObj);
            if (atRiskCoEmpPmDbGetManyEmployeeIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // 取得 注意 的 員工信箱
            // db, 核心-員工-資訊-資料庫-取得多筆[信箱]
            var atRiskCoEmpInfoDbGetManyEmailReqObj = new CoEmpInfDbGetManyEmailReqMdl()
            {
                EmployeeIdList = atRiskCoEmpPmDbGetManyEmployeeIdRspObj.EmployeeProjectMemberList
                    .Select(x => x.EmployeeID)
                    .Distinct()
                    .ToList(),
            };
            var atRiskCoEmpInfoDbGetManyEmailRspObj = await this._coEmpInfoDb.GetManyEmailAsync(atRiskCoEmpInfoDbGetManyEmailReqObj);
            if (atRiskCoEmpInfoDbGetManyEmailRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // 組成 注意 通知內容
            var atRiskSb = new StringBuilder();
            atRiskSb.AppendLine("以下是注意的專案清單:<br/>");
            foreach (var employeeProjectItem in atRiskCoEmpPrjDbGetManyIdRspObj.EmployeeProjectList)
            {
                atRiskSb.AppendLine($"專案網址: {this._gsApiAppSetting.PlatformConfig.ClientFrontSite}/work/project/detail/{employeeProjectItem.EmployeeProjectID}<br/>");
            }

            // 發送 注意 通知
            var atRiskCmnSmtpSendReqObj = new CmnSmtpSendReqMdl()
            {
                Config = new CmnSmtpConfig
                {
                    Host = this._gsApiAppSetting.SmtpConfig.Host,
                    Port = this._gsApiAppSetting.SmtpConfig.Port,
                    EnableSsl = this._gsApiAppSetting.SmtpConfig.EnableSsl,
                    SenderEmail = this._gsApiAppSetting.SmtpConfig.FromEmail,
                    SenderName = this._gsApiAppSetting.SmtpConfig.FromName,
                    Username = this._gsApiAppSetting.SmtpConfig.Username,
                    Password = this._gsApiAppSetting.SmtpConfig.Password
                },
                ReceiverList = atRiskCoEmpInfoDbGetManyEmailRspObj.EmployeeList
                    .Select(x => x.EmployeeEmail)
                    .Distinct()
                    .ToList(),
                Subject = "有風險的專案",
                Body = atRiskSb.ToString(),
                IsHtml = true,
            };
            var atRiskCmnSmtpSendRspObj = await this._cmnSmtp.SendAsync(atRiskCmnSmtpSendReqObj);

        }

        #endregion

    }

}
