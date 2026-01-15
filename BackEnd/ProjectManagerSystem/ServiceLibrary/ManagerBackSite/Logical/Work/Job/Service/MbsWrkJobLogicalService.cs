using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.EmployeeProjectStoneJobBucket;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Project.Format;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Service;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Service;

/// <summary>管理者後台-工作-工項-邏輯服務</summary>
public class MbsWrkJobLogicalService : IMbsWrkJobLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsWrkJobLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-公司-資料庫服務</summary>
    private readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-專案-資料庫服務</summary>
    private readonly ICoEmpProjectDbService _coEmpProjectDb;

    /// <summary>核心-員工-專案成員-資料庫服務</summary>
    private readonly ICoEmpProjectMemberDbService _coEmpProjectMemberDb;

    /// <summary>核心-員工-專案里程碑-資料庫服務</summary>
    private readonly ICoEmpProjectStoneDbService _coEmpProjectStoneDb;

    /// <summary>核心-員工-專案里程碑工項-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobDbService _coEmpProjectStoneJobDb;

    /// <summary>核心-員工-專案里程碑工項清單-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobBucketDbService _coEmpProjectStoneJobBucketDb;

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobExecutorDbService _coEmpProjectStoneJobExecutorDb;

    /// <summary>核心-員工-專案里程碑工項工作-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobWorkDbService _coEmpProjectStoneJobWorkDb;

    /// <summary>核心-員工-專案里程碑工項工作檔案-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobWorkFileDbService _coEmpProjectStoneJobWorkFileDb;

    #endregion

    #region ManagerBackSite

    /// <summary>管理者後台-通用-邏輯服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #endregion

    #region This

    /// <summary>原子-管理者後台-列舉</summary>
    private readonly DbAtomMenuEnum _atomMenu;

    #endregion

    /// <summary>建構式</summary>
    public MbsWrkJobLogicalService(
        ILogger<MbsWrkJobLogicalService> logger,
        // Core Manager
        ICoMgrCompanyDbService coMgrCompanyDb,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpProjectDbService coEmpProjectDb,
        ICoEmpProjectMemberDbService coEmpProjectMemberDb,
        ICoEmpProjectStoneDbService coEmpProjectStoneDb,
        ICoEmpProjectStoneJobDbService coEmpProjectStoneJobDb,
        ICoEmpProjectStoneJobBucketDbService coEmpProjectStoneJobBucketDb,
        ICoEmpProjectStoneJobExecutorDbService coEmpProjectStoneJobExecutorDb,
        ICoEmpProjectStoneJobWorkDbService coEmpProjectStoneJobWorkDb,
        ICoEmpProjectStoneJobWorkFileDbService coEmpProjectStoneJobWorkFileDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrCompanyDb = coMgrCompanyDb;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpProjectDb = coEmpProjectDb;
        this._coEmpProjectMemberDb = coEmpProjectMemberDb;
        this._coEmpProjectStoneDb = coEmpProjectStoneDb;
        this._coEmpProjectStoneJobDb = coEmpProjectStoneJobDb;
        this._coEmpProjectStoneJobBucketDb = coEmpProjectStoneJobBucketDb;
        this._coEmpProjectStoneJobExecutorDb = coEmpProjectStoneJobExecutorDb;
        this._coEmpProjectStoneJobWorkDb = coEmpProjectStoneJobWorkDb;
        this._coEmpProjectStoneJobWorkFileDb = coEmpProjectStoneJobWorkFileDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._atomMenu = DbAtomMenuEnum.WorkJob;
    }

    #region 專案里程碑工項

    /// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項</summary>
    public async Task<MbsWrkJobLgcGetManyProjectStoneJobRspMdl> GetManyProjectStoneJobAsync(MbsWrkJobLgcGetManyProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcGetManyProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項工作-取得筆數從[管理者後台-工作-工項]
        var coEmpPsjwDbGetCountFromMbsWrkJobReqObj = new CoEmpPsjwDbGetCountFromMbsWrkJobReqMdl()
        {
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobStatus = reqObj.EmployeeProjectStoneJobStatus,
            StartEmployeeProjectStoneJobEndTime = reqObj.StartEmployeeProjectStoneJobEndTime,
            EndEmployeeProjectStoneJobEndTime = reqObj.EndEmployeeProjectStoneJobEndTime,
        };
        var coEmpPsjwDbGetCountFromMbsWrkJobRspObj = await this._coEmpProjectStoneJobWorkDb.GetCountFromMbsWrkJobAsync(coEmpPsjwDbGetCountFromMbsWrkJobReqObj);
        if (coEmpPsjwDbGetCountFromMbsWrkJobRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果比數為0，直接回傳空
        if (coEmpPsjwDbGetCountFromMbsWrkJobRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeeProjectStoneJobList = new List<MbsWrkJobLgcGetManyProjectStoneJobRspItemJobMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工項]
        var coEmpPsjwDbGetManyFromMbsWrkJobReqObj = new CoEmpPsjwDbGetManyFromMbsWrkJobReqMdl()
        {
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobStatus = reqObj.EmployeeProjectStoneJobStatus,
            StartEmployeeProjectStoneJobEndTime = reqObj.StartEmployeeProjectStoneJobEndTime,
            EndEmployeeProjectStoneJobEndTime = reqObj.EndEmployeeProjectStoneJobEndTime,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coEmpPsjwDbGetManyFromMbsWrkJobRspObj = await this._coEmpProjectStoneJobWorkDb.GetManyFromMbsWrkJobAsync(coEmpPsjwDbGetManyFromMbsWrkJobReqObj);
        if (coEmpPsjwDbGetManyFromMbsWrkJobRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案-取得多筆[名稱]
        var coEmpPrjDbGetManyNameReqObj = new CoEmpPrjDbGetManyNameReqMdl()
        {
            EmployeeProjectIdList = coEmpPsjwDbGetManyFromMbsWrkJobRspObj.EmployeeProjectStoneJobList
                .Select(x => x.EmployeeProjectID)
                .Distinct()
                .ToList(),
        };
        var coEmpPrjDbGetManyNameRspObj = await this._coEmpProjectDb.GetManyNameAsync(coEmpPrjDbGetManyNameReqObj);
        if (coEmpPrjDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑-取得多筆[名稱]
        var coEmpPsDbGetManyNameReqObj = new CoEmpPsDbGetManyNameReqMdl()
        {
            EmployeeProjectStoneIdList = coEmpPsjwDbGetManyFromMbsWrkJobRspObj.EmployeeProjectStoneJobList
                .Select(x => x.EmployeeProjectStoneID)
                .Distinct()
                .ToList(),
        };
        var coEmpPsDbGetManyNameRspObj = await this._coEmpProjectStoneDb.GetManyNameAsync(coEmpPsDbGetManyNameReqObj);
        if (coEmpPsDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfoDbGetManyNameReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = coEmpPsjwDbGetManyFromMbsWrkJobRspObj.EmployeeProjectStoneJobList
                .SelectMany(x => x.EmployeeList)
                .Select(x => x.EmployeeID)
                .Distinct()
                .ToList(),
        };
        var coEmpInfoDbGetManyNameRspObj = await this._coEmpInfoDb.GetManyNameAsync(coEmpInfoDbGetManyNameReqObj);
        if (coEmpInfoDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneJobList =
            (
                from job in coEmpPsjwDbGetManyFromMbsWrkJobRspObj.EmployeeProjectStoneJobList
                from prj in coEmpPrjDbGetManyNameRspObj.EmployeeProjectList
                    .Where(prjx => prjx.EmployeeProjectID == job.EmployeeProjectID)
                from stn in coEmpPsDbGetManyNameRspObj.EmployeeProjectStoneList
                    .Where(stnx => stnx.EmployeeProjectStoneID == job.EmployeeProjectStoneID)
                select new MbsWrkJobLgcGetManyProjectStoneJobRspItemJobMdl()
                {
                    EmployeeProjectName = prj.EmployeeProjectName,
                    EmployeeProjectStoneName = stn.EmployeeProjectStoneName,
                    EmployeeProjectStoneJobID = job.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobName = job.EmployeeProjectStoneJobName,
                    EmployeeProjectStoneJobStatus = job.EmployeeProjectStoneJobStatus,
                    EmployeeProjectStoneJobStartTime = job.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = job.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobExecutorList =
                        (
                            from emp in job.EmployeeList
                            from name in coEmpInfoDbGetManyNameRspObj.EmployeeList
                                .Where(namex => namex.EmployeeID == emp.EmployeeID)
                            select new MbsWrkJobLgcGetManyProjectStoneJobRspItemExecutorMdl()
                            {
                                EmployeeProjectStoneJobExecutorName = name.EmployeeName,
                            }
                        ).ToList(),
                }
            ).ToList();
        rspObj.TotalCount = coEmpPsjwDbGetCountFromMbsWrkJobRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項</summary>
    public async Task<MbsWrkJobLgcGetProjectStoneJobRspMdl> GetProjectStoneJobAsync(MbsWrkJobLgcGetProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcGetProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // db, 核心-員工-專案里程碑工項執行者-取得多筆
        var coEmpPsjeDbGetManyReqObj = new CoEmpPsjeDbGetManyReqMdl()
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var coEmpPsjeDbGetManyRspObj = await this._coEmpProjectStoneJobExecutorDb.GetManyAsync(coEmpPsjeDbGetManyReqObj);
        if (coEmpPsjeDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷專案成員權限
        var isLoginEmployeeInProjectMember = coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
            .Any(x => x.EmployeeID == mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID);

        if (isLoginEmployeeInProjectMember == false)
        {
            this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項-取得
        var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl()
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
        if (coEmpPsjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案-取得
        var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
        };
        var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
        if (coEmpPrjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑-取得
        var coEmpPsDbGetReqObj = new CoEmpPsDbGetReqMdl()
        {
            EmployeeProjectStoneID = coEmpPsjDbGetRspObj.EmployeeProjectStoneID,
        };
        var coEmpPsDbGetRspObj = await this._coEmpProjectStoneDb.GetAsync(coEmpPsDbGetReqObj);
        if (coEmpPsDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfDbGetManyNameReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                .Select(x => x.EmployeeID)
                .Distinct()
                .ToList(),
        };
        var coEmpInfDbGetManyNameRspObj = await this._coEmpInfoDb.GetManyNameAsync(coEmpInfDbGetManyNameReqObj);
        if (coEmpInfDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項清單-資料庫-取得多筆
        var coEmpPsjbDbGetManyReqObj = new CoEmpPsjbDbGetManyReqMdl()
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var coEmpPsjbDbGetManyRspObj = await this._coEmpProjectStoneJobBucketDb.GetManyAsync(coEmpPsjbDbGetManyReqObj);
        if (coEmpPsjbDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項工作-資料庫-取得多筆
        var coEmpPsjwDbGetManyReqObj = new CoEmpPsjwDbGetManyReqMdl()
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var coEmpPsjwDbGetManyRspObj = await this._coEmpProjectStoneJobWorkDb.GetManyAsync(coEmpPsjwDbGetManyReqObj);
        if (coEmpPsjwDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項工作檔案-資料庫-取得多筆從[管理者後台-工作-工項]
        var coEmpPsjwfDbGetManyReqObj = new CoEmpPsjwfDbGetManyReqMdl()
        {
            EmployeeProjectStoneJobWorkIDList = coEmpPsjwDbGetManyRspObj.EmployeeProjectStoneJobWorkList
            .Select(x => x.EmployeeProjectStoneJobWorkID)
            .ToList(),
        };
        var coEmpPsjwfDbGetManyRspObj = await this._coEmpProjectStoneJobWorkFileDb.GetManyAsync(coEmpPsjwfDbGetManyReqObj);
        if (coEmpPsjwfDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }


        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectName = coEmpPrjDbGetRspObj.EmployeeProjectName;
        rspObj.EmployeeProjectStartTime = coEmpPrjDbGetRspObj.EmployeeProjectStartTime;
        rspObj.EmployeeProjectEndTime = coEmpPrjDbGetRspObj.EmployeeProjectEndTime;
        rspObj.EmployeeProjectStoneName = coEmpPsDbGetRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = coEmpPsDbGetRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = coEmpPsDbGetRspObj.EmployeeProjectStoneEndTime;
        rspObj.EmployeeProjectStoneJobName = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobName;
        rspObj.EmployeeProjectStoneJobStartTime = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobStartTime;
        rspObj.EmployeeProjectStoneJobEndTime = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobEndTime;
        rspObj.EmployeeProjectStoneJobStatus = coEmpPsjDbGetRspObj.AtomEmployeeProjectStatus;
        rspObj.EmployeeProjectStoneJobRemark = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobRemark;
        rspObj.EmployeeProjectStoneJobExecutorList =
            (
                from excutor in coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                from empName in coEmpInfDbGetManyNameRspObj.EmployeeList
                    .Where(x => x.EmployeeID == excutor.EmployeeID)
                select new MbsWrkJobLgcGetProjectStoneJobRspItemExecutorMdl()
                {
                    EmployeeProjectStoneJobExecutorName = empName.EmployeeName,
                }
            ).ToList();
        rspObj.EmployeeProjectStoneJobBucketList = coEmpPsjbDbGetManyRspObj.EmployeeProjectStoneJobBucketList
            .Select(x => new MbsWrkJobLgcGetProjectStoneJobRspItemBucketMdl()
            {
                EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
            })
            .ToList();
        rspObj.EmployeeProjectStoneJobWorkList =
            (
                from work in coEmpPsjwDbGetManyRspObj.EmployeeProjectStoneJobWorkList
                from empName in coEmpInfDbGetManyNameRspObj.EmployeeList
                    .Where(x => x.EmployeeID == work.EmployeeID)
                select new MbsWrkJobLgcGetProjectStoneJobRspItemWorkMdl()
                {
                    EmployeeProjectStoneJobWorkID = work.EmployeeProjectStoneJobWorkID,
                    EmployeeProjectStoneJobWorkStartTime = work.EmployeeProjectStoneJobWorkStartTime,
                    EmployeeProjectStoneJobWorkEndTime = work.EmployeeProjectStoneJobWorkEndTime,
                    EmployeeName = empName.EmployeeName,
                    EmployeeProjectStoneJobWorkRemark = work.EmployeeProjectStoneJobWorkRemark,
                }
            ).ToList();
        rspObj.EmployeeProjectStoneJobWorkFileList = coEmpPsjwfDbGetManyRspObj.EmployeeProjectStoneJobWorkFileList
            .Select(x => new MbsWrkJobLgcGetProjectStoneJobRspItemFileMdl()
            {
                EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl,
            })
            .ToList();

        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-邏輯服務-更新專案里程碑工項</summary>
    public async Task<MbsWrkJobLgcUpdateProjectStoneJobRspMdl> UpdateProjectStoneJobAsync(MbsWrkJobLgcUpdateProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcUpdateProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項-資料庫-取得
        var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
        if (coEmpPsjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項-資料庫-更新
        var coEmpPsjDbUpdateReqObj = new CoEmpPsjDbUpdateReqMdl
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark,
        };
        var coEmpPsjDbUpdateRspObj = await this._coEmpProjectStoneJobDb.UpdateAsync(coEmpPsjDbUpdateReqObj);
        if (coEmpPsjDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        if (reqObj.EmployeeProjectStoneJobBucketList != null)
        {
            // 更新工項清單(工項清單-ID, >0:更新, -1:新增)

            // db, 核心-員工-專案里程碑工項清單-資料庫-取得多筆
            var coEmpPsjbDbGetManyReqObj = new CoEmpPsjbDbGetManyReqMdl
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            };
            var coEmpPsjbDbGetManyRspObj = await this._coEmpProjectStoneJobBucketDb.GetManyAsync(coEmpPsjbDbGetManyReqObj);
            if (coEmpPsjbDbGetManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 取得該刪掉的專案里程碑工項清單列表
            var deleteBucketList = coEmpPsjbDbGetManyRspObj.EmployeeProjectStoneJobBucketList
                .Where(x => !reqObj.EmployeeProjectStoneJobBucketList
                                    .Select(y => y.EmployeeProjectStoneJobBucketID)
                                    .Contains(x.EmployeeProjectStoneJobBucketID))
                .ToList();
            if (deleteBucketList.Count > 0)
            {
                // db, 核心-員工-專案里程碑工項清單-資料庫-移除多筆
                var coEmpPsjbDbRemoveManyReqObj = new CoEmpPsjbDbRemoveManyReqMdl
                {
                    EmployeeProjectStoneJobBucketIdList = deleteBucketList
                        .Select(x => x.EmployeeProjectStoneJobBucketID)
                        .Distinct()
                        .ToList()
                };
                var coEmpPsjbDbRemoveManyRspObj = await this._coEmpProjectStoneJobBucketDb.RemoveManyAsync(coEmpPsjbDbRemoveManyReqObj);
                if (coEmpPsjbDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
            }

            // 取得該更新的專案里程碑工項清單列表
            var updateBucketList = reqObj.EmployeeProjectStoneJobBucketList
                .Where(x => x.EmployeeProjectStoneJobBucketID > 0)
                .ToList();
            foreach (var updateBucketItem in updateBucketList)
            {
                // db, 核心-員工-專案里程碑工項清單-資料庫-更新
                var coEmpPsjbDbUpdateReqObj = new CoEmpPsjbDbUpdateReqMdl
                {
                    EmployeeProjectStoneJobBucketID = updateBucketItem.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketName = updateBucketItem.EmployeeProjectStoneJobBucketName,
                    EmployeeProjectStoneJobBucketIsFinish = updateBucketItem.EmployeeProjectStoneJobBucketIsFinish
                };
                var coEmpPsjbDbUpdateRspObj = await this._coEmpProjectStoneJobBucketDb.UpdateAsync(coEmpPsjbDbUpdateReqObj);
                if (coEmpPsjbDbUpdateRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(updateBucketItem)}");
                    return rspObj;
                }
            }

            // 取得該新增的專案里程碑工項清單列表
            var addBucketList = reqObj.EmployeeProjectStoneJobBucketList
                .Where(x => x.EmployeeProjectStoneJobBucketID < 0)
                .ToList();

            if (addBucketList != null && addBucketList.Count > 0)
            {
                // 如果使用者有提供要新增的清單項目，就新增這些項目
                var coEmpPsjbDbAddManyReqObj = new CoEmpPsjbDbAddManyReqMdl
                {
                    EmployeeProjectStoneJobBucketList = addBucketList
                        .Select(x => new CoEmpPsjbDbAddManyReqItemMdl
                        {
                            EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
                            EmployeeProjectStoneID = coEmpPsjDbGetRspObj.EmployeeProjectStoneID,
                            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
                            EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                            EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                        })
                        .ToList()
                };
                var coEmpPsjbDbAddManyRspObj = await this._coEmpProjectStoneJobBucketDb.AddManyAsync(coEmpPsjbDbAddManyReqObj);
                if (coEmpPsjbDbAddManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
            }
        }

        // logical, 檢查與更新專案狀態
        var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
        {
            EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
        };
        await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 專案里程碑工項工作

    /// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項工作</summary>
    public async Task<MbsWrkJobLgcGetProjectStoneJobWorkRspMdl> GetProjectStoneJobWorkAsync(MbsWrkJobLgcGetProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcGetProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項工作-取得
        var coEmpPsjwDbGetReqObj = new CoEmpPsjwDbGetReqMdl()
        {
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
        };
        var coEmpPsjwDbGetRspObj = await this._coEmpProjectStoneJobWorkDb.GetAsync(coEmpPsjwDbGetReqObj);
        if (coEmpPsjwDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 取得工作檔案
        var coEmpPsjwfDbGetManyFromMbsWrkJobReqObj = new CoEmpPsjwfDbGetManyReqMdl()
        {
            EmployeeProjectStoneJobWorkIDList = new List<int> { reqObj.EmployeeProjectStoneJobWorkID },
        };
        var coEmpPsjwfDbGetManyFromMbsWrkJobRspObj = await this._coEmpProjectStoneJobWorkFileDb.GetManyAsync(coEmpPsjwfDbGetManyFromMbsWrkJobReqObj);
        if (coEmpPsjwfDbGetManyFromMbsWrkJobRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #region 工項

        string employeeProjectStoneJobRemark = null;
        var employeeProjectStoneJobBucketList = new List<MbsWrkJobLgcGetProjectStoneJobWorkReqItemBucketMdl>();

        // 專案/里程碑/工項時間資訊
        string employeeProjectName = null;
        DateTimeOffset employeeProjectStartTime = default;
        DateTimeOffset employeeProjectEndTime = default;
        string employeeProjectStoneName = null;
        DateTimeOffset employeeProjectStoneStartTime = default;
        DateTimeOffset employeeProjectStoneEndTime = default;
        string employeeProjectStoneJobName = null;
        DateTimeOffset employeeProjectStoneJobStartTime = default;
        DateTimeOffset employeeProjectStoneJobEndTime = default;

        // 判斷是否有工項ID
        if (coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobID.HasValue)
        {
            // 有工項ID，取得相關資料

            // db, 核心-員工-專案里程碑工項-取得
            var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl()
            {
                EmployeeProjectStoneJobID = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobID.Value,
            };
            var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
            if (coEmpPsjDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-專案-取得
            var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
            {
                EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
            };
            var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
            if (coEmpPrjDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-專案里程碑-取得
            var coEmpPsDbGetReqObj = new CoEmpPsDbGetReqMdl()
            {
                EmployeeProjectStoneID = coEmpPsjDbGetRspObj.EmployeeProjectStoneID,
            };
            var coEmpPsDbGetRspObj = await this._coEmpProjectStoneDb.GetAsync(coEmpPsDbGetReqObj);
            if (coEmpPsDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-專案里程碑工項清單-資料庫-取得多筆
            var coEmpPsjbDbGetManyReqObj = new CoEmpPsjbDbGetManyReqMdl()
            {
                EmployeeProjectStoneJobID = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobID.Value,
            };
            var coEmpPsjbDbGetManyRspObj = await this._coEmpProjectStoneJobBucketDb.GetManyAsync(coEmpPsjbDbGetManyReqObj);
            if (coEmpPsjbDbGetManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 專案資訊
            employeeProjectName = coEmpPrjDbGetRspObj.EmployeeProjectName;
            employeeProjectStartTime = coEmpPrjDbGetRspObj.EmployeeProjectStartTime;
            employeeProjectEndTime = coEmpPrjDbGetRspObj.EmployeeProjectEndTime;

            // 里程碑資訊
            employeeProjectStoneName = coEmpPsDbGetRspObj.EmployeeProjectStoneName;
            employeeProjectStoneStartTime = coEmpPsDbGetRspObj.EmployeeProjectStoneStartTime;
            employeeProjectStoneEndTime = coEmpPsDbGetRspObj.EmployeeProjectStoneEndTime;

            // 工項資訊
            employeeProjectStoneJobName = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobName;
            employeeProjectStoneJobStartTime = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobStartTime;
            employeeProjectStoneJobEndTime = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobEndTime;
            employeeProjectStoneJobRemark = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobRemark;
            employeeProjectStoneJobBucketList = coEmpPsjbDbGetManyRspObj.EmployeeProjectStoneJobBucketList
                .Select(x => new MbsWrkJobLgcGetProjectStoneJobWorkReqItemBucketMdl()
                {
                    EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                    EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                })
                .ToList();
        }

        #endregion

        // 組成回應
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        // 工項
        rspObj.EmployeeProjectStoneJobID = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobID;
        rspObj.EmployeeProjectStoneJobRemark = employeeProjectStoneJobRemark;
        rspObj.EmployeeProjectStoneJobBucketList = employeeProjectStoneJobBucketList;
        // 工作
        rspObj.EmployeeProjectStoneJobWorkStartTime = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobWorkStartTime;
        rspObj.EmployeeProjectStoneJobWorkEndTime = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobWorkEndTime;
        rspObj.EmployeeProjectStoneJobWorkRemark = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobWorkRemark?.Trim();
        rspObj.EmployeeProjectStoneJobWorkFileList = coEmpPsjwfDbGetManyFromMbsWrkJobRspObj.EmployeeProjectStoneJobWorkFileList
            .Select(x => new MbsWrkJobLgcGetProjectStoneJobWorkReqItemFileMdl()
            {
                EmployeeProjectStoneJobWorkFileID = x.EmployeeProjectStoneJobWorkFileID,
                EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl,
            })
            .ToList();
        // 專案
        rspObj.EmployeeProjectName = employeeProjectName;
        rspObj.EmployeeProjectStartTime = employeeProjectStartTime;
        rspObj.EmployeeProjectEndTime = employeeProjectEndTime;
        // 里程碑
        rspObj.EmployeeProjectStoneName = employeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = employeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = employeeProjectStoneEndTime;
        // 工項時間
        rspObj.EmployeeProjectStoneJobName = employeeProjectStoneJobName;
        rspObj.EmployeeProjectStoneJobStartTime = employeeProjectStoneJobStartTime;
        rspObj.EmployeeProjectStoneJobEndTime = employeeProjectStoneJobEndTime;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-邏輯服務-新增專案里程碑工項工作</summary>
    public async Task<MbsWrkJobLgcAddProjectStoneJobWorkRspMdl> AddProjectStoneJobWorkAsync(MbsWrkJobLgcAddProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcAddProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        int? employeeProjectID = null;
        int? employeeProjectStoneID = null;

        // 判斷是否有工項ID
        if (reqObj.EmployeeProjectStoneJobID.HasValue)
        {
            // 有，拿到其他ID

            // db, 核心-員工-專案里程碑工項-資料庫-取得
            var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl()
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID.Value,
            };
            var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
            if (coEmpPsjDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            employeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID;
            employeeProjectStoneID = coEmpPsjDbGetRspObj.EmployeeProjectStoneID;
        }

        #region 新增 新增專案里程碑工項工作

        // db, 核心-員工-專案里程碑工項工作-新增
        var coEmpPsjwDbAddReqObj = new CoEmpPsjwDbAddReqMdl()
        {
            EmployeeProjectID = employeeProjectID,
            EmployeeProjectStoneID = employeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            EmployeeProjectStoneJobWorkStartTime = reqObj.EmployeeProjectStoneJobWorkStartTime,
            EmployeeProjectStoneJobWorkEndTime = reqObj.EmployeeProjectStoneJobWorkEndTime,
            EmployeeProjectStoneJobWorkRemark = reqObj.EmployeeProjectStoneJobWorkRemark,
        };
        var coEmpPsjwDbAddRspObj = await this._coEmpProjectStoneJobWorkDb.AddAsync(coEmpPsjwDbAddReqObj);
        if (coEmpPsjwDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 新增 工作檔案
        if (reqObj.EmployeeProjectStoneJobWorkFileList != null
            && reqObj.EmployeeProjectStoneJobWorkFileList.Any())
        {
            var coEmpPsjwfDbAddManyReqObj = new CoEmpPsjwfDbAddManyReqMdl()
            {
                EmployeeProjectStoneJobWorkFileList = reqObj.EmployeeProjectStoneJobWorkFileList
                    .Select(x => new CoEmpPsjwfDbAddReqMdl
                    {
                        EmployeeProjectID = employeeProjectID,
                        EmployeeProjectStoneID = employeeProjectStoneID,
                        EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobWorkID = coEmpPsjwDbAddRspObj.EmployeeProjectStoneJobWorkID,
                        EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl,
                    })
                    .ToList()
            };
            var coEmpPsjwfDbAddManyRspObj = await this._coEmpProjectStoneJobWorkFileDb.AddManyAsync(coEmpPsjwfDbAddManyReqObj);
            if (coEmpPsjwfDbAddManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        #endregion

        #region 修改 專案里程碑工項

        // 判斷是否有工項ID
        if (reqObj.EmployeeProjectStoneJobID.HasValue)
        {
            // 沒有，代表是新增工作紀錄

            // db, 核心-員工-專案里程碑工項-資料庫-更新
            var coEmpPsjDbUpdateReqObj = new CoEmpPsjDbUpdateReqMdl()
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID.Value,
                EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark,
            };
            var coEmpPsjDbUpdateRspObj = await this._coEmpProjectStoneJobDb.UpdateAsync(coEmpPsjDbUpdateReqObj);
            if (coEmpPsjDbUpdateRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 處理清單
            if (reqObj.EmployeeProjectStoneJobBucketList != null
                && reqObj.EmployeeProjectStoneJobBucketList.Any())
            {
                // foreach, 核心-員工-專案里程碑工項清單-資料庫-更新
                foreach (var employeeProjectStoneJobBucketItem in reqObj.EmployeeProjectStoneJobBucketList)
                {
                    // db, 核心-員工-專案里程碑工項清單-資料庫-更新
                    var coEmpPsjbDbUpdateReqObj = new CoEmpPsjbDbUpdateReqMdl()
                    {
                        EmployeeProjectStoneJobBucketID = employeeProjectStoneJobBucketItem.EmployeeProjectStoneJobBucketID,
                        EmployeeProjectStoneJobBucketIsFinish = employeeProjectStoneJobBucketItem.EmployeeProjectStoneJobBucketIsFinish,
                    };
                    var coEmpPsjbDbUpdateRspObj = await this._coEmpProjectStoneJobBucketDb.UpdateAsync(coEmpPsjbDbUpdateReqObj);
                    if (coEmpPsjbDbUpdateRspObj == default)
                    {
                        this._logger.LogError(
                            $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                            $", item: {JsonSerializer.Serialize(employeeProjectStoneJobBucketItem)}");
                        return rspObj;
                    }

                }
            }

            // logical, 檢查與更新專案狀態
            var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
            {
                EmployeeProjectID = employeeProjectID.Value,
            };
            await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);

        }

        #endregion

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-邏輯服務-更新專案里程碑工項工作</summary>
    public async Task<MbsWrkJobLgcUpdateProjectStoneJobWorkRspMdl> UpdateProjectStoneJobWorkAsync(MbsWrkJobLgcUpdateProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcUpdateProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // db, 核心-員工-專案里程碑工項工作-取得
        var coEmpPsjwDbGetReqObj = new CoEmpPsjwDbGetReqMdl()
        {
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
        };
        var coEmpPsjwDbGetRspObj = await this._coEmpProjectStoneJobWorkDb.GetAsync(coEmpPsjwDbGetReqObj);
        if (coEmpPsjwDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷登入者是否為工作紀錄的建立者
        if (coEmpPsjwDbGetRspObj.EmployeeID != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 更新專案里程碑工項

        // 判斷是否需要更新工項（工項ID不為null時才執行）
        if (reqObj.EmployeeProjectStoneJobID.HasValue)
        {
            // db, 取得專案里程碑工項
            var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl()
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID.Value,
            };
            var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
            if (coEmpPsjDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 更新工項備註
            var coEmpPsjDbUpdateReqObj = new CoEmpPsjDbUpdateReqMdl()
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID.Value,
                EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark?.Trim(),
            };
            var coEmpPsjDbUpdateRspObj = await this._coEmpProjectStoneJobDb.UpdateAsync(coEmpPsjDbUpdateReqObj);
            if (coEmpPsjDbUpdateRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 更新工項清單完成狀態
            if (reqObj.EmployeeProjectStoneJobBucketList != null
                && reqObj.EmployeeProjectStoneJobBucketList.Any())
            {
                // foreach, db, 核心-員工-專案里程碑工項清單-資料庫-更新
                foreach (var bucketItem in reqObj.EmployeeProjectStoneJobBucketList)
                {
                    // db, 核心-員工-專案里程碑工項清單-資料庫-更新
                    var coEmpPsjbDbUpdateReqObj = new CoEmpPsjbDbUpdateReqMdl()
                    {
                        EmployeeProjectStoneJobBucketID = bucketItem.EmployeeProjectStoneJobBucketID,
                        EmployeeProjectStoneJobBucketIsFinish = bucketItem.EmployeeProjectStoneJobBucketIsFinish,
                    };
                    var coEmpPsjbDbUpdateRspObj = await this._coEmpProjectStoneJobBucketDb.UpdateAsync(coEmpPsjbDbUpdateReqObj);
                    if (coEmpPsjbDbUpdateRspObj == default)
                    {
                        this._logger.LogError(
                            $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                            $", bucketItem: {JsonSerializer.Serialize(bucketItem)}");
                        return rspObj;
                    }
                }
            }

            // logical, 檢查與更新專案狀態
            var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
            {
                EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
            };
            await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);
        }

        #endregion

        #region 更新專案里程碑工項工作

        // db, 核心-員工-專案里程碑工項工作-更新
        var coEmpPsjwDbUpdateReqObj = new CoEmpPsjwDbUpdateReqMdl()
        {
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
            EmployeeProjectStoneJobWorkStartTime = reqObj.EmployeeProjectStoneJobWorkStartTime,
            EmployeeProjectStoneJobWorkEndTime = reqObj.EmployeeProjectStoneJobWorkEndTime,
            EmployeeProjectStoneJobWorkRemark = reqObj.EmployeeProjectStoneJobWorkRemark,
        };
        var coEmpPsjwDbUpdateRspObj = await this._coEmpProjectStoneJobWorkDb.UpdateAsync(coEmpPsjwDbUpdateReqObj);
        if (coEmpPsjwDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否更新工作檔案
        if (reqObj.EmployeeProjectStoneJobWorkFileList != null)
        {
            // db, 取得現有工作檔案
            var coEmpPsjwfDbGetManyReqObj = new CoEmpPsjwfDbGetManyReqMdl()
            {
                EmployeeProjectStoneJobWorkIDList = new List<int> { reqObj.EmployeeProjectStoneJobWorkID },
            };
            var coEmpPsjwfDbGetManyRspObj = await this._coEmpProjectStoneJobWorkFileDb.GetManyAsync(coEmpPsjwfDbGetManyReqObj);
            if (coEmpPsjwfDbGetManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 移除
            var deleteFileIdList =
                (
                    from db in coEmpPsjwfDbGetManyRspObj.EmployeeProjectStoneJobWorkFileList
                    from req in reqObj.EmployeeProjectStoneJobWorkFileList
                        .Where(reqx => reqx.EmployeeProjectStoneJobWorkFileID == db.EmployeeProjectStoneJobWorkFileID)
                    .DefaultIfEmpty()
                    where req == null
                    select new
                    {
                        db.EmployeeProjectStoneJobWorkFileID,
                    }
                ).ToList();
            if (deleteFileIdList.Any())
            {
                // db, 核心-員工-專案里程碑工項工作檔案-移除多筆
                var coEmpPsjwfDbRemoveManyReqObj = new CoEmpPsjwfDbRemoveManyReqMdl()
                {
                    EmployeeProjectStoneJobWorkFileIdList = deleteFileIdList
                        .Select(x => x.EmployeeProjectStoneJobWorkFileID)
                        .Distinct()
                        .ToList(),
                };
                var coEmpPsjwfDbRemoveManyRspObj = await this._coEmpProjectStoneJobWorkFileDb.RemoveManyAsync(coEmpPsjwfDbRemoveManyReqObj);
                if (coEmpPsjwfDbRemoveManyRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", deleteFileIdList: {JsonSerializer.Serialize(deleteFileIdList)}");
                    return rspObj;
                }
            }

            // 更新
            var updateFileList = reqObj.EmployeeProjectStoneJobWorkFileList
                .Where(x => x.EmployeeProjectStoneJobWorkFileID > 0)
                .ToList();
            foreach (var updateFileItem in updateFileList)
            {
                // db, 核心-員工-專案里程碑工項工作檔案-更新
                var coEmpPsjwfDbUpdateReqObj = new CoEmpPsjwfDbUpdateReqMdl()
                {
                    EmployeeProjectStoneJobWorkFileID = updateFileItem.EmployeeProjectStoneJobWorkFileID,
                    EmployeeProjectStoneJobWorkFileUrl = updateFileItem.EmployeeProjectStoneJobWorkFileUrl,
                };
                var coEmpPsjwfDbUpdateRspObj = await this._coEmpProjectStoneJobWorkFileDb.UpdateAsync(coEmpPsjwfDbUpdateReqObj);
                if (coEmpPsjwfDbUpdateRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", fileItem: {JsonSerializer.Serialize(updateFileItem)}");
                    return rspObj;
                }
            }

            // 新增
            var addFileList = reqObj.EmployeeProjectStoneJobWorkFileList
                .Where(x => x.EmployeeProjectStoneJobWorkFileID < 0)
                .Select(x => new CoEmpPsjwfDbAddReqMdl()
                {
                    EmployeeProjectID = coEmpPsjwDbGetRspObj.EmployeeProjectID,
                    EmployeeProjectStoneID = coEmpPsjwDbGetRspObj.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobID = coEmpPsjwDbGetRspObj.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
                    EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl,
                })
                .ToList();
            if (addFileList.Any())
            {
                var coEmpPsjwfDbAddManyReqObj = new CoEmpPsjwfDbAddManyReqMdl()
                {
                    EmployeeProjectStoneJobWorkFileList = addFileList,
                };
                var coEmpPsjwfDbAddManyRspObj = await this._coEmpProjectStoneJobWorkFileDb.AddManyAsync(coEmpPsjwfDbAddManyReqObj);
                if (coEmpPsjwfDbAddManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
            }
        }

        #endregion

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項工作</summary>
    public async Task<MbsWrkJobLgcGetManyProjectStoneJobWorkRspMdl> GetManyProjectStoneJobWorkAsync(MbsWrkJobLgcGetManyProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcGetManyProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self
            && mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項工作-取得[筆數]從[管理者後台-工作-工作紀錄]
        var coEmpPsjwDbGetCountFromMbsWrkJobRecReqObj = new CoEmpPsjwDbGetCountFromMbsWrkJobRecReqMdl()
        {
            StartEmployeeProjectStoneJobWorkStartTime = reqObj.StartEmployeeProjectStoneJobWorkStartTime,
            EndEmployeeProjectStoneJobWorkEndTime = reqObj.EndEmployeeProjectStoneJobWorkEndTime,
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPsjwDbGetCountFromMbsWrkJobRecRspObj = await this._coEmpProjectStoneJobWorkDb.GetCountFromMbsWrkJobRecAsync(coEmpPsjwDbGetCountFromMbsWrkJobRecReqObj);
        if (coEmpPsjwDbGetCountFromMbsWrkJobRecRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coEmpPsjwDbGetCountFromMbsWrkJobRecRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeeProjectStoneJobWorkList = new List<MbsWrkJobLgcGetManyProjectStoneJobWorkRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工作紀錄]
        var coEmpPsjwDbGetManyFromMbsWrkJobRecReqObj = new CoEmpPsjwDbGetManyFromMbsWrkJobRecReqMdl()
        {
            StartEmployeeProjectStoneJobWorkStartTime = reqObj.StartEmployeeProjectStoneJobWorkStartTime,
            EndEmployeeProjectStoneJobWorkEndTime = reqObj.EndEmployeeProjectStoneJobWorkEndTime,
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coEmpPsjwDbGetManyFromMbsWrkJobRecRspObj = await this._coEmpProjectStoneJobWorkDb.GetManyFromMbsWrkJobRecAsync(coEmpPsjwDbGetManyFromMbsWrkJobRecReqObj);
        if (coEmpPsjwDbGetManyFromMbsWrkJobRecRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneJobWorkList = coEmpPsjwDbGetManyFromMbsWrkJobRecRspObj.EmployeeProjectStoneJobWorkList
            .Select(x => new MbsWrkJobLgcGetManyProjectStoneJobWorkRspItemMdl()
            {
                EmployeeProjectStoneJobWorkID = x.EmployeeProjectStoneJobWorkID,
                EmployeeProjectStoneJobWorkStartTime = x.EmployeeProjectStoneJobWorkStartTime,
                EmployeeProjectStoneJobWorkEndTime = x.EmployeeProjectStoneJobWorkEndTime,
                EmployeeProjectStoneJobWorkRemark = x.EmployeeProjectStoneJobWorkRemark,
                EmployeeProjectID = x.EmployeeProjectID ?? 0,
                EmployeeProjectName = x.EmployeeProjectName,
                EmployeeProjectStoneID = x.EmployeeProjectStoneID ?? 0,
                EmployeeProjectStoneName = x.EmployeeProjectStoneName,
                EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID ?? 0,
                EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName,
                EmployeeName = x.EmployeeName,
            })
            .ToList();
        rspObj.TotalCount = coEmpPsjwDbGetCountFromMbsWrkJobRecRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-邏輯服務-移除專案里程碑工項工作</summary>
    public async Task<MbsWrkJobLgcRemoveProjectStoneJobWorkRspMdl> RemoveProjectStoneJobWorkAsync(MbsWrkJobLgcRemoveProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobLgcRemoveProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // db, 核心-員工-專案里程碑工項工作-取得
        var coEmpPsjwDbGetReqObj = new CoEmpPsjwDbGetReqMdl()
        {
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
        };
        var coEmpPsjwDbGetRspObj = await this._coEmpProjectStoneJobWorkDb.GetAsync(coEmpPsjwDbGetReqObj);
        if (coEmpPsjwDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷登入者是否為工作紀錄的建立者
        if (coEmpPsjwDbGetRspObj.EmployeeID != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項工作-移除
        var coEmpPsjwDbRemoveReqObj = new CoEmpPsjwDbRemoveReqMdl()
        {
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
        };
        var coEmpPsjwDbRemoveRspObj = await this._coEmpProjectStoneJobWorkDb.RemoveAsync(coEmpPsjwDbRemoveReqObj);
        if (coEmpPsjwDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

}
