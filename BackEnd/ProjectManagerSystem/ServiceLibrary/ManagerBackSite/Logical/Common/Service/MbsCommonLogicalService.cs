using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Permission.Format;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Employee.DB.Project.Format;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Service;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Common.Service;

/// <summary>管理者後台-通用-邏輯服務</summary>
public class MbsCommonLogicalService : IMbsCommonLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsCommonLogicalService> _logger;

    #region Core Employee

    /// <summary>核心-員工-專案-資料庫服務</summary>
    private readonly ICoEmpProjectDbService _coEmpProjectDb;

    /// <summary>核心-員工-專案里程碑-資料庫服務</summary>
    private readonly ICoEmpProjectStoneDbService _coEmpProjectStoneDb;

    /// <summary>核心-員工-專案里程碑工項-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobDbService _coEmpProjectStoneJobDb;

    /// <summary>核心-員工-專案里程碑工項清單-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobBucketDbService _coEmpProjectStoneJobBucketDb;

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobExecutorDbService _coEmpProjectStoneJobExecutorDb;

    #endregion

    #region Core Manager

    /// <summary>核心-員工-目錄權限-資料庫服務</summary>
    private readonly ICoEmpPermissionDbService _coEmpPermissionDb;

    /// <summary>核心-員工-登入資訊-記憶體服務</summary>
    private readonly ICoEmpLoginInfoMemoryService _coEmpLoginInfoMemory;

    #endregion

    /// <summary>建構式</summary>
    public MbsCommonLogicalService(
        ILogger<MbsCommonLogicalService> logger,
        // Core Employee
        ICoEmpProjectDbService coEmpProjectDb,
        ICoEmpProjectStoneDbService coEmpProjectStoneDb,
        ICoEmpProjectStoneJobDbService coEmpProjectStoneJobDb,
        ICoEmpProjectStoneJobBucketDbService coEmpProjectStoneJobBucketDb,
        ICoEmpProjectStoneJobExecutorDbService coEmpProjectStoneJobExecutorDb,
        // Core Manager
        ICoEmpPermissionDbService coEmpPermissionDb,
        ICoEmpLoginInfoMemoryService coEmpLoginInfoMemory)
    {
        this._logger = logger;
        // Core Employee
        this._coEmpProjectDb = coEmpProjectDb;
        this._coEmpProjectStoneDb = coEmpProjectStoneDb;
        this._coEmpProjectStoneJobDb = coEmpProjectStoneJobDb;
        this._coEmpProjectStoneJobBucketDb = coEmpProjectStoneJobBucketDb;
        this._coEmpProjectStoneJobExecutorDb = coEmpProjectStoneJobExecutorDb;
        // Core Manager
        this._coEmpPermissionDb = coEmpPermissionDb;
        this._coEmpLoginInfoMemory = coEmpLoginInfoMemory;
    }

    /// <summary>管理者後台-通用-邏輯-取得登入使用者資訊</summary>
    public async Task<MbsCmnLgcGetLoginUserInfoRspMdl> GetLoginUserInfoAsync(MbsCmnLgcGetLoginUserInfoReqMdl reqObj)
    {
        var rspObj = new MbsCmnLgcGetLoginUserInfoRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-目錄權限-資料庫-取得
        var coEmpPmnDbGetReqObj = new CoEmpPmnDbGetReqMdl()
        {
            EmployeeID = coEmpLiMemGetRspObj.EmployeeID,
            AtomMenu = reqObj.AtomMenu,
        };
        var coEmpPmnDbGetRspObj = await this._coEmpPermissionDb.GetAsync(coEmpPmnDbGetReqObj);
        if (coEmpPmnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeID = coEmpLiMemGetRspObj.EmployeeID;
        rspObj.ManagerRegionID = coEmpPmnDbGetRspObj.ManagerRegionID;
        rspObj.AtomPermissionKindIdView = coEmpPmnDbGetRspObj.AtomPermissionKindIdView;
        rspObj.AtomPermissionKindIdCreate = coEmpPmnDbGetRspObj.AtomPermissionKindIdCreate;
        rspObj.AtomPermissionKindIdEdit = coEmpPmnDbGetRspObj.AtomPermissionKindIdEdit;
        rspObj.AtomPermissionKindIdDelete = coEmpPmnDbGetRspObj.AtomPermissionKindIdDelete;
        return rspObj;
    }

    /// <summary>管理者後台-通用-邏輯-登出</summary>
    public async Task<MbsCmnLgcLogoutRspMdl> LogoutAsync(MbsCmnLgcLogoutReqMdl reqObj)
    {
        var rspObj = new MbsCmnLgcLogoutRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // memory, 核心-員工-登入資訊-記憶體-取得多筆[登入令牌]
        var coEmpLiMemGetManyLoginTokenReqObj = new CoEmpLiMemGetManyLoginTokenReqMdl()
        {
            EmployeeID = reqObj.EmployeeID,
        };
        var coEmpLiMemGetManyLoginTokenRspObj = this._coEmpLoginInfoMemory.GetManyLoginToken(coEmpLiMemGetManyLoginTokenReqObj);
        if (coEmpLiMemGetManyLoginTokenRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // memory, 核心-員工-登入資訊-記憶體-移除多筆
        var coEmpLiMemRemoveManyReqObj = new CoEmpLiMemRemoveManyReqMdl()
        {
            EmployeeLoginTokenList = coEmpLiMemGetManyLoginTokenRspObj.EmployeeLoginInfoList
                .Select(x => x.EmployeeLoginToken)
                .Distinct()
                .ToList(),
            EmployeeIdList = new List<int> { reqObj.EmployeeID }
        };
        var coEmpLiMemRemoveManyRspObj = this._coEmpLoginInfoMemory.RemoveMany(coEmpLiMemRemoveManyReqObj);
        if (coEmpLiMemRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return await Task.FromResult(rspObj);
    }

    /// <summary>管理者後台-通用-邏輯-檢查與更新專案狀態</summary>
    public async Task CheckAndUpdateProjectStatusAsync(MbsCmnLgcCheckAndUpdateProjectStatusReqMdl reqObj)
    {
        // db, 核心-員工-專案-取得
        var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
        if (coEmpPrjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return;
        }

        // db, 核心-員工-專案里程碑-取得多筆
        var coEmpPsDbGetManyReqObj = new CoEmpPsDbGetManyReqMdl()
        {
            EmployeeProjectIdList = new List<int>
            {
                reqObj.EmployeeProjectID,
            }
        };
        var coEmpPsDbGetManyRspObj = await this._coEmpProjectStoneDb.GetManyAsync(coEmpPsDbGetManyReqObj);
        if (coEmpPsDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return;
        }

        // db, 核心-員工-專案里程碑工項-取得多筆
        var coEmpPsjDbGetManyReqObj = new CoEmpPsjDbGetManyReqMdl()
        {
            EmployeeProjectIdList = new List<int>
            {
                reqObj.EmployeeProjectID,
            },
        };
        var coEmpPsjDbGetManyRspObj = await this._coEmpProjectStoneJobDb.GetManyAsync(coEmpPsjDbGetManyReqObj);
        if (coEmpPsjDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return;
        }

        // db, 核心-員工-專案里程碑工項清單-資料庫-取得多筆
        var coEmpPsjbDbGetManyReqObj = new CoEmpPsjbDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPsjbDbGetManyRspObj = await this._coEmpProjectStoneJobBucketDb.GetManyAsync(coEmpPsjbDbGetManyReqObj);
        if (coEmpPsjbDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return;
        }

        // db, 核心-員工-專案里程碑工項執行者-取得多筆
        var coEmpPsjeDbGetManyReqObj = new CoEmpPsjeDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPsjeDbGetManyRspObj = await this._coEmpProjectStoneJobExecutorDb.GetManyAsync(coEmpPsjeDbGetManyReqObj);
        if (coEmpPsjeDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return;
        }

        // 判斷與更新里程碑狀態
        var stoneStatusList = new List<DbAtomEmployeeProjectStatusEnum>();
        foreach (var employeeProjectStoneItem in coEmpPsDbGetManyRspObj.EmployeeProjectStoneList)
        {
            // 取得該里程碑的所有工項
            var employeeProjectStoneTaskList = coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList
                .Where(x => x.EmployeeProjectStoneID == employeeProjectStoneItem.EmployeeProjectStoneID)
                .ToList();

            // 判斷與更新工項狀態
            var taskStatusList = new List<DbAtomEmployeeProjectStatusEnum>();
            foreach (var employeeProjectStoneTaskItem in employeeProjectStoneTaskList)
            {
                // 取得該工項的所有清單
                var employeeProjectStoneTaskBucketList = coEmpPsjbDbGetManyRspObj.EmployeeProjectStoneJobBucketList
                    .Where(x => x.EmployeeProjectStoneJobID == employeeProjectStoneTaskItem.EmployeeProjectStoneJobID)
                    .Select(x => new CoEmpPsjbDbGetManyRspItemMdl()
                    {
                        EmployeeProjectID = x.EmployeeProjectID,
                        EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                        EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                        EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                        EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                    })
                    .ToList();

                // 判斷該工項的所有清單是否已完成
                var isAllBucketFinished =
                    employeeProjectStoneTaskBucketList != null
                    && employeeProjectStoneTaskBucketList.Count > 0
                    && employeeProjectStoneTaskBucketList.All(x => x.EmployeeProjectStoneJobBucketIsFinish == true);

                // 判斷該工項是否有執行者
                var hasExecutor = coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                    .Any(x => x.EmployeeProjectStoneJobID == employeeProjectStoneTaskItem.EmployeeProjectStoneJobID);

                // 判斷工項狀態
                var taskStatus = DbAtomEmployeeProjectStatusEnum.Undefined;
                if (isAllBucketFinished)
                {
                    // 所有工項清單已完成 => 已完成
                    taskStatus = DbAtomEmployeeProjectStatusEnum.Completed;
                }
                else if (hasExecutor == false)
                {
                    // 沒有執行者 => 未指派
                    taskStatus = DbAtomEmployeeProjectStatusEnum.NotAssigned;
                }
                else if (employeeProjectStoneItem.EmployeeProjectStoneEndTime <= DateTimeOffset.UtcNow)
                {
                    // 專案里程碑結束時間 <= 現在，延遲
                    taskStatus = DbAtomEmployeeProjectStatusEnum.Delayed;
                }
                else if (employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime <= DateTimeOffset.UtcNow
                    && DateTimeOffset.UtcNow < employeeProjectStoneItem.EmployeeProjectStoneEndTime)
                {
                    // 工項結束時間 <= 現在 < 專案里程碑結束時間，風險
                    taskStatus = DbAtomEmployeeProjectStatusEnum.AtRisk;
                }
                else if (employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime <= DateTimeOffset.UtcNow
                    && DateTimeOffset.UtcNow < employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime)
                {
                    // 工項開始時間 <= 現在 < 工項結束時間，準時
                    taskStatus = DbAtomEmployeeProjectStatusEnum.OnSchedule;
                }
                else if (DateTimeOffset.UtcNow < employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime)
                {
                    // 現在 < 工項開始時間，未開始
                    taskStatus = DbAtomEmployeeProjectStatusEnum.NotStarted;
                }
                else
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item1: {JsonSerializer.Serialize(employeeProjectStoneItem)}" +
                        $", item2: {JsonSerializer.Serialize(employeeProjectStoneTaskItem)}");
                    return;
                }

                // 判斷是否一樣狀態
                if (employeeProjectStoneTaskItem.AtomEmployeeProjectStatus != taskStatus)
                {
                    // 不一樣，更新

                    // db, 核心-員工-專案里程碑工項-資料庫-更新
                    var coEmpPsjDbUpdateReqObj = new CoEmpPsjDbUpdateReqMdl()
                    {
                        EmployeeProjectStoneJobID = employeeProjectStoneTaskItem.EmployeeProjectStoneJobID,
                        AtomEmployeeProjectStatus = taskStatus,
                    };
                    var coEmpPsjDbUpdateRspObj = await this._coEmpProjectStoneJobDb.UpdateAsync(coEmpPsjDbUpdateReqObj);
                    if (coEmpPsjDbUpdateRspObj == default)
                    {
                        this._logger.LogError(
                            $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                            $", item1: {JsonSerializer.Serialize(employeeProjectStoneItem)}" +
                            $", item2: {JsonSerializer.Serialize(employeeProjectStoneTaskItem)}");
                        return;
                    }
                }

                // 儲存該工項的狀態
                taskStatusList.Add(taskStatus);
            }

            // 判斷里程碑狀態
            var stoneStatus = DbAtomEmployeeProjectStatusEnum.Undefined;
            if (taskStatusList.All(x => x == DbAtomEmployeeProjectStatusEnum.Completed))
            {
                stoneStatus = DbAtomEmployeeProjectStatusEnum.Completed;
            }
            else if (taskStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.NotAssigned))
            {
                stoneStatus = DbAtomEmployeeProjectStatusEnum.NotAssigned;
            }
            else if (taskStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.Delayed))
            {
                stoneStatus = DbAtomEmployeeProjectStatusEnum.Delayed;
            }
            else if (taskStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.AtRisk))
            {
                stoneStatus = DbAtomEmployeeProjectStatusEnum.AtRisk;
            }
            else if (taskStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.OnSchedule))
            {
                stoneStatus = DbAtomEmployeeProjectStatusEnum.OnSchedule;
            }
            else if (taskStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.NotStarted))
            {
                stoneStatus = DbAtomEmployeeProjectStatusEnum.NotStarted;
            }
            else
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", item: {JsonSerializer.Serialize(employeeProjectStoneItem)}");
                return;
            }

            // 判斷是否一樣狀態
            if (employeeProjectStoneItem.AtomEmployeeProjectStatus != stoneStatus)
            {
                // 不一樣，更新

                // db, 核心-員工-專案里程碑-資料庫-更新
                var coEmpPsDbUpdateReqObj = new CoEmpPsDbUpdateReqMdl()
                {
                    EmployeeProjectStoneID = employeeProjectStoneItem.EmployeeProjectStoneID,
                    AtomEmployeeProjectStatus = stoneStatus,
                };
                var coEmpPsDbUpdateRspObj = await this._coEmpProjectStoneDb.UpdateAsync(coEmpPsDbUpdateReqObj);
                if (coEmpPsDbUpdateRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return;
                }
            }

            // 儲存該里程碑的狀態
            stoneStatusList.Add(stoneStatus);
        }

        // 判斷專案狀態
        var projectStatus = DbAtomEmployeeProjectStatusEnum.Undefined;
        if (stoneStatusList.All(x => x == DbAtomEmployeeProjectStatusEnum.Completed))
        {
            projectStatus = DbAtomEmployeeProjectStatusEnum.Completed;
        }
        else if (stoneStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.NotAssigned))
        {
            projectStatus = DbAtomEmployeeProjectStatusEnum.NotAssigned;
        }
        else if (stoneStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.Delayed))
        {
            projectStatus = DbAtomEmployeeProjectStatusEnum.Delayed;
        }
        else if (stoneStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.AtRisk))
        {
            projectStatus = DbAtomEmployeeProjectStatusEnum.AtRisk;
        }
        else if (stoneStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.OnSchedule))
        {
            projectStatus = DbAtomEmployeeProjectStatusEnum.OnSchedule;
        }
        else if (stoneStatusList.Any(x => x == DbAtomEmployeeProjectStatusEnum.NotStarted))
        {
            projectStatus = DbAtomEmployeeProjectStatusEnum.NotStarted;
        }
        else
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return;
        }

        // 判斷是否一樣狀態
        if (coEmpPrjDbGetRspObj.AtomEmployeeProjectStatus != projectStatus)
        {
            // 不一樣，更新

            // db, 核心-員工-專案-資料庫-更新
            var coEmpPrjDbUpdateReqObj = new CoEmpPrjDbUpdateReqMdl()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                AtomEmployeeProjectStatus = projectStatus,
            };
            var coEmpPrjDbUpdateRspObj = await this._coEmpProjectDb.UpdateAsync(coEmpPrjDbUpdateReqObj);
            if (coEmpPrjDbUpdateRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return;
            }
        }

    }

}
