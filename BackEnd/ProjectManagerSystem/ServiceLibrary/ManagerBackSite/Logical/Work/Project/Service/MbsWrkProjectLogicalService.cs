using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.Employee;
using DataModelLibrary.Database.EmployeeProjectMember;
using DataModelLibrary.Database.EmployeeProjectStoneJobBucket;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Project.Format;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Format;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Service;
using ServiceLibrary.Core.Employee.DB.ProjectExpense.Format;
using ServiceLibrary.Core.Employee.DB.ProjectExpense.Service;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Format;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Service;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Format;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Service;
using ServiceLibrary.Core.Manager.DB.Company.Format;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.EipV1.Http.Format;
using ServiceLibrary.EipV1.Http.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Service;

/// <summary>管理者後台-工作-專案-邏輯服務</summary>
public class MbsWrkProjectLogicalService : IMbsWrkProjectLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsWrkProjectLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-公司-資料庫服務</summary>
    private readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-專案-資料庫服務</summary>
    private readonly ICoEmpProjectDbService _coEmpProjectDb;

    /// <summary>核心-員工-專案契約-資料庫服務</summary>
    private readonly ICoEmpProjectContractDbService _coEmpProjectContractDb;

    /// <summary>核心-員工-專案支出-資料庫服務</summary>
    private readonly ICoEmpProjectExpenseDbService _coEmpProjectExpenseDb;

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

    /// <summary>核心-員工-專案工作計劃書-資料庫服務</summary>
    private readonly ICoEmpProjectWorkDbService _coEmpProjectWorkDb;

    #endregion

    #region ManagerBackSite

    /// <summary>管理者後台-通用-邏輯服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #endregion

    #region EipV1

    /// <summary>EipV1-Http-服務</summary>
    private readonly IEipV1HttpService _eipV1Http;

    #endregion

    #region This

    /// <summary>原子-管理者後台-列舉</summary>
    private readonly DbAtomMenuEnum _atomMenu;

    #endregion

    /// <summary>建構式</summary>
    public MbsWrkProjectLogicalService(
        ILogger<MbsWrkProjectLogicalService> logger,
        // Core Manager
        ICoMgrCompanyDbService coMgrCompanyDb,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpProjectDbService coEmpProjectDb,
        ICoEmpProjectContractDbService coEmpProjectContractDb,
        ICoEmpProjectExpenseDbService coEmpProjectExpenseDb,
        ICoEmpProjectMemberDbService coEmpProjectMemberDb,
        ICoEmpProjectStoneDbService coEmpProjectStoneDb,
        ICoEmpProjectStoneJobDbService coEmpProjectStoneJobDb,
        ICoEmpProjectStoneJobBucketDbService coEmpProjectStoneJobBucketDb,
        ICoEmpProjectStoneJobExecutorDbService coEmpProjectStoneJobExecutorDb,
        ICoEmpProjectWorkDbService coEmpProjectWorkDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical,
        // EipV1-Http
        IEipV1HttpService eipV1Http)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrCompanyDb = coMgrCompanyDb;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpProjectDb = coEmpProjectDb;
        this._coEmpProjectContractDb = coEmpProjectContractDb;
        this._coEmpProjectExpenseDb = coEmpProjectExpenseDb;
        this._coEmpProjectMemberDb = coEmpProjectMemberDb;
        this._coEmpProjectStoneDb = coEmpProjectStoneDb;
        this._coEmpProjectStoneJobDb = coEmpProjectStoneJobDb;
        this._coEmpProjectStoneJobBucketDb = coEmpProjectStoneJobBucketDb;
        this._coEmpProjectStoneJobExecutorDb = coEmpProjectStoneJobExecutorDb;
        this._coEmpProjectWorkDb = coEmpProjectWorkDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // EipV1-Http
        this._eipV1Http = eipV1Http;
        // This
        this._atomMenu = DbAtomMenuEnum.WorkProject;
    }

    #region 儀錶板

    /// <summary>管理者後台-工作-專案-邏輯服務-取得儀錶板</summary>
    public async Task<MbsWrkPrjLgcGetDashboardRspMdl> GetDashboardAsync(MbsWrkPrjLgcGetDashboardReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetDashboardRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-取得儀錶板從[管理後台-工作-專案]
        var coEmpPrjDbGetDashboardFromMbsWrkPrjReqObj = new CoEmpPrjDbGetDashboardFromMbsWrkPrjReqMdl()
        {
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            AtomEmployeeProjectStatusList = new List<DbAtomEmployeeProjectStatusEnum>()
            {
                DbAtomEmployeeProjectStatusEnum.NotAssigned,
                DbAtomEmployeeProjectStatusEnum.AtRisk,
                DbAtomEmployeeProjectStatusEnum.Delayed,
            },
        };
        var coEmpPrjDbGetDashboardFromMbsWrkPrjRspObj = await this._coEmpProjectDb.GetDashboardFromMbsWrkPrjAsync(coEmpPrjDbGetDashboardFromMbsWrkPrjReqObj);
        if (coEmpPrjDbGetDashboardFromMbsWrkPrjRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得多筆[名稱]
        var coMgrCmpDbGetManyNameReqObj = new CoMgrCmpDbGetManyNameReqMdl()
        {
            ManagerCompanyIdList = coEmpPrjDbGetDashboardFromMbsWrkPrjRspObj.EmployeeProjectList
                .Select(x => x.ManagerCompanyID)
                .Distinct()
                .ToList(),
        };
        var coMgrCmpDbGetManyNameRspObj = await this._coMgrCompanyDb.GetManyNameAsync(coMgrCmpDbGetManyNameReqObj);
        if (coMgrCmpDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.DelayedEmployeeProjectList =
            (
                from d in coEmpPrjDbGetDashboardFromMbsWrkPrjRspObj.EmployeeProjectList
                    .Where(dx => dx.AtomEmployeeProjectStatus == DbAtomEmployeeProjectStatusEnum.Delayed)
                from mc in coMgrCmpDbGetManyNameRspObj.ManagerCompanyList
                    .Where(mcx => mcx.ManagerCompanyID == d.ManagerCompanyID)
                select new MbsWrkPrjLgcGetDashboardRspItemMdl()
                {
                    EmployeeProjectID = d.EmployeeProjectID,
                    ManagerCompanyName = mc.ManagerCompanyName,
                    EmployeeProjectName = d.EmployeeProjectName,
                    EmployeeProjectStartTime = d.EmployeeProjectStartTime,
                    EmployeeProjectEndTime = d.EmployeeProjectEndTime,
                }
            ).ToList();
        rspObj.AtRiskEmployeeProjectList =
            (
                from a in coEmpPrjDbGetDashboardFromMbsWrkPrjRspObj.EmployeeProjectList
                    .Where(ax => ax.AtomEmployeeProjectStatus == DbAtomEmployeeProjectStatusEnum.AtRisk)
                from mc in coMgrCmpDbGetManyNameRspObj.ManagerCompanyList
                    .Where(mcx => mcx.ManagerCompanyID == a.ManagerCompanyID)
                select new MbsWrkPrjLgcGetDashboardRspItemMdl()
                {
                    EmployeeProjectID = a.EmployeeProjectID,
                    ManagerCompanyName = mc.ManagerCompanyName,
                    EmployeeProjectName = a.EmployeeProjectName,
                    EmployeeProjectStartTime = a.EmployeeProjectStartTime,
                    EmployeeProjectEndTime = a.EmployeeProjectEndTime,
                }
            ).ToList();
        rspObj.NotAssignedEmployeeProjectList =
            (
                from n in coEmpPrjDbGetDashboardFromMbsWrkPrjRspObj.EmployeeProjectList
                    .Where(nx => nx.AtomEmployeeProjectStatus == DbAtomEmployeeProjectStatusEnum.NotAssigned)
                from mc in coMgrCmpDbGetManyNameRspObj.ManagerCompanyList
                    .Where(mcx => mcx.ManagerCompanyID == n.ManagerCompanyID)
                select new MbsWrkPrjLgcGetDashboardRspItemMdl()
                {
                    EmployeeProjectID = n.EmployeeProjectID,
                    ManagerCompanyName = mc.ManagerCompanyName,
                    EmployeeProjectName = n.EmployeeProjectName,
                    EmployeeProjectStartTime = n.EmployeeProjectStartTime,
                    EmployeeProjectEndTime = n.EmployeeProjectEndTime,
                }
            ).ToList();
        return rspObj;
    }

    #endregion

    #region 專案

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案</summary>
    public async Task<MbsWrkPrjLgcGetManyProjectRspMdl> GetManyProjectAsync(MbsWrkPrjLgcGetManyProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetManyProjectRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-取得筆數從[管理後台-工作-專案]
        var coEmpPrjDbGetCountFromMbsWrkPrjReqObj = new CoEmpPrjDbGetCountFromMbsWrkPrjReqMdl()
        {
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            AtomEmployeeProjectStatus = reqObj.AtomEmployeeProjectStatus,
            EmployeeProjectName = reqObj.EmployeeProjectName,
        };
        var coEmpPrjDbGetCountFromMbsWrkPrjRspObj = await this._coEmpProjectDb.GetCountFromMbsWrkPrjAsync(coEmpPrjDbGetCountFromMbsWrkPrjReqObj);
        if (coEmpPrjDbGetCountFromMbsWrkPrjRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果比數為0，直接回傳空
        if (coEmpPrjDbGetCountFromMbsWrkPrjRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeeProjectList = new List<MbsWrkPrjLgcGetManyProjectRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-專案-取得多筆從[管理後台-工作-專案]
        var coEmpPrjDbGetManyFromMbsWrkPrjReqObj = new CoEmpPrjDbGetManyFromMbsWrkPrjReqMdl()
        {
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            AtomEmployeeProjectStatus = reqObj.AtomEmployeeProjectStatus,
            EmployeeProjectName = reqObj.EmployeeProjectName,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coEmpPrjDbGetManyFromMbsWrkPrjRspObj = await this._coEmpProjectDb.GetManyFromMbsWrkPrjAsync(coEmpPrjDbGetManyFromMbsWrkPrjReqObj);
        if (coEmpPrjDbGetManyFromMbsWrkPrjRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectList = coEmpPrjDbGetManyFromMbsWrkPrjRspObj.EmployeeProjectList
            .Select(x => new MbsWrkPrjLgcGetManyProjectRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                AtomEmployeeProjectStatus = x.AtomEmployeeProjectStatus,
                EmployeeProjectName = x.EmployeeProjectName,
                ManagerCompanyName = x.ManagerCompanyName,
                EmployeeProjectStartTime = x.EmployeeProjectStartTime,
                EmployeeProjectEndTime = x.EmployeeProjectEndTime,
            })
            .ToList();
        rspObj.TotalCount = coEmpPrjDbGetCountFromMbsWrkPrjRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案</summary>
    public async Task<MbsWrkPrjLgcGetProjectRspMdl> GetProjectAsync(MbsWrkPrjLgcGetProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetProjectRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷專案成員權限
        var isLoginEmployeeInProjectMember = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => x.EmployeeID == mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID);

        if (isLoginEmployeeInProjectMember == false)
        {
            this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-取得
        var coEmpPmDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPmDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPmDbGetReqObj);
        if (coEmpPmDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案契約-取得多筆
        var coEmpPcDbGetManyReqObj = new CoEmpPcDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectContractIsNewest = true,
        };
        var coEmpPcDbGetManyRspObj = await this._coEmpProjectContractDb.GetManyAsync(coEmpPcDbGetManyReqObj);
        if (coEmpPcDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 契約只會有一筆是最新的，不是0筆也不是多筆
        var employeeProjectContract = coEmpPcDbGetManyRspObj.EmployeeProjectContractList.SingleOrDefault();

        // db, 核心-員工-專案工作計劃書-取得多筆
        var coEmpPwDbGetManyReqObj = new CoEmpPwDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectContractIsNewest = true,
        };
        var coEmpPwDbGetManyRspObj = await this._coEmpProjectWorkDb.GetManyAsync(coEmpPwDbGetManyReqObj);
        if (coEmpPwDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 工作計劃書只會有一筆是最新的，不是0筆也不是多筆
        var employeeProjectWork = coEmpPwDbGetManyRspObj.EmployeeProjectWorkList.SingleOrDefault();

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱][角色ID]
        var coEmpInfDbGetManyRegionDepartmentEmployeeReqObj = new CoEmpInfDbGetManyRegionDepartmentEmployeeReqMdl()
        {
            EmployeeIdList = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
                .Select(x => x.EmployeeID)
                .Distinct()
                .ToList(),
        };
        var coEmpInfDbGetManyRegionDepartmentEmployeeRspObj = await this._coEmpInfoDb.GetManyRegionDepartmentEmployeeAsync(coEmpInfDbGetManyRegionDepartmentEmployeeReqObj);
        if (coEmpInfDbGetManyRegionDepartmentEmployeeRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得[名稱]
        var coMgrCmpDbGetNameReqObj = new CoMgrCmpDbGetNameReqMdl()
        {
            ManagerCompanyID = coEmpPmDbGetRspObj.ManagerCompanyID,
        };
        var coMgrCmpDbGetNameRspObj = await this._coMgrCompanyDb.GetNameAsync(coMgrCmpDbGetNameReqObj);
        if (coMgrCmpDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.AtomEmployeeProjectStatus = coEmpPmDbGetRspObj.AtomEmployeeProjectStatus;
        rspObj.EmployeeProjectName = coEmpPmDbGetRspObj.EmployeeProjectName;
        rspObj.EmployeeProjectCode = coEmpPmDbGetRspObj.EmployeeProjectCode;
        rspObj.EmployeeProjectStartTime = coEmpPmDbGetRspObj.EmployeeProjectStartTime;
        rspObj.EmployeeProjectEndTime = coEmpPmDbGetRspObj.EmployeeProjectEndTime;
        rspObj.ManagerCompanyName = coMgrCmpDbGetNameRspObj.ManagerCompanyName;
        rspObj.EmployeeProjectContractUrl = employeeProjectContract?.EmployeeProjectContractUrl ?? string.Empty;
        rspObj.EmployeeProjectContractCreateTime = employeeProjectContract?.EmployeeProjectContractCreateTime;
        rspObj.EmployeeProjectWorkUrl = employeeProjectWork?.EmployeeProjectWorkUrl ?? string.Empty;
        rspObj.EmployeeProjectWorkCreateTime = employeeProjectWork?.EmployeeProjectWorkCreateTime;
        rspObj.EmployeeProjectMemberList =
            (
                from epm in coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
                from e in coEmpInfDbGetManyRegionDepartmentEmployeeRspObj.EmployeeList
                    .Where(ex => ex.EmployeeID == epm.EmployeeID)
                select new MbsWrkPrjLgcGetProjectRspItemMdl()
                {
                    EmployeeProjectMemberID = epm.EmployeeProjectMemberID,
                    EmployeeProjectMemberRole = epm.EmployeeProjectMemberRole,
                    ManagerRegionName = e.ManagerRegionName,
                    ManagerDepartmentName = e.ManagerDepartmentName,
                    EmployeeName = e.EmployeeName,
                    EmployeeID = e.EmployeeID,
                }
        ).ToList();

        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-新增專案</summary>
    public async Task<MbsWrkPrjLgcAddProjectRspMdl> AddProjectAsync(MbsWrkPrjLgcAddProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcAddProjectRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-資料庫-是否存在
        var coEmpPrjDbExistReqObj = new CoEmpPrjDbExistReqMdl()
        {
            EmployeeProjectCode = reqObj.EmployeeProjectCode,
        };
        var coEmpPrjDbExistRspObj = await this._coEmpProjectDb.ExistAsync(coEmpPrjDbExistReqObj);
        if (coEmpPrjDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPrjDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.ProjectCodeDuplicate;
            return rspObj;
        }

        // db, 核心-員工-專案-資料庫-新增
        var coEmpPrjDbAddReqObj = new CoEmpPrjDbAddReqMdl()
        {
            EmployeePipelineID = null,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.NotAssigned,
            EmployeeProjectCode = reqObj.EmployeeProjectCode,
            EmployeeProjectStartTime = reqObj.EmployeeProjectStartTime,
            EmployeeProjectEndTime = reqObj.EmployeeProjectEndTime,
            EmployeeProjectName = reqObj.EmployeeProjectName,
        };
        var coEmpPrjDbAddRspObj = await this._coEmpProjectDb.AddAsync(coEmpPrjDbAddReqObj);
        if (coEmpPrjDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否新增專案契約
        if (!string.IsNullOrWhiteSpace(reqObj.EmployeeProjectContractUrl))
        {
            // db, 核心-員工-專案契約-資料庫-新增
            var coEmpPcDbAddReqObj = new CoEmpPcDbAddReqMdl()
            {
                EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                EmployeeProjectContractUrl = reqObj.EmployeeProjectContractUrl,
                EmployeeProjectContractIsNewest = true,
            };
            var coEmpPcDbAddRspObj = await this._coEmpProjectContractDb.AddAsync(coEmpPcDbAddReqObj);
            if (coEmpPcDbAddRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // this, 恢復新增專案
                await this.RecoveryAddProjectAsync(coEmpPrjDbAddRspObj.EmployeeProjectID);
                return rspObj;
            }
        }

        // 判斷是否新增專案工作計劃書
        if (!string.IsNullOrWhiteSpace(reqObj.EmployeeProjectWorkUrl))
        {
            // db, 核心-員工-專案工作計劃書-資料庫-新增
            var coEmpPwDbAddReqObj = new CoEmpPwDbAddReqMdl()
            {
                EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                EmployeeProjectWorkUrl = reqObj.EmployeeProjectWorkUrl,
                EmployeeProjectWorkIsNewest = true,
            };
            var coEmpPwDbAddRspObj = await this._coEmpProjectWorkDb.AddAsync(coEmpPwDbAddReqObj);
            if (coEmpPwDbAddRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // this, 恢復新增專案
                await this.RecoveryAddProjectAsync(coEmpPrjDbAddRspObj.EmployeeProjectID);
                return rspObj;
            }
        }

        // 專案成員，輸入的人員 + 老闆
        var employeeProjectMemberList = reqObj.EmployeeProjectMemberList
            .Select(x => new CoEmpPmemDbAddManyReqItemMdl()
            {
                EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                EmployeeID = x.EmployeeID,
                EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
            })
            .ToList();
        employeeProjectMemberList.Add(new CoEmpPmemDbAddManyReqItemMdl()
        {
            EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
            EmployeeID = DbEmployeeConst.GeneralManager.ID,
            EmployeeProjectMemberRole = DbEmployeeProjectMemberRoleEnum.GeneralManager,
        });

        // db, 核心-員工-專案成員-資料庫-新增多筆
        var coEmpPmemDbAddManyReqObj = new CoEmpPmemDbAddManyReqMdl()
        {
            EmployeeProjectMemberList = employeeProjectMemberList
                .Select(employee => new CoEmpPmemDbAddManyReqItemMdl
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                    EmployeeID = employee.EmployeeID,
                    EmployeeProjectMemberRole = employee.EmployeeProjectMemberRole
                })
                .ToList(),
        };
        var coEmpPmemDbAddManyRspObj = await this._coEmpProjectMemberDb.AddManyAsync(coEmpPmemDbAddManyReqObj);
        if (coEmpPmemDbAddManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // this, 恢復新增專案
            await this.RecoveryAddProjectAsync(coEmpPrjDbAddRspObj.EmployeeProjectID);
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案</summary>
    public async Task<MbsWrkPrjLgcUpdateProjectRspMdl> UpdateProjectAsync(MbsWrkPrjLgcUpdateProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcUpdateProjectRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-資料庫-是否存在（檢查專案代碼是否重複，排除自己）
        var coEmpPrjDbExistReqObj = new CoEmpPrjDbExistReqMdl()
        {
            EmployeeProjectCode = reqObj.EmployeeProjectCode,
        };
        var coEmpPrjDbExistRspObj = await this._coEmpProjectDb.ExistAsync(coEmpPrjDbExistReqObj);
        if (coEmpPrjDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPrjDbExistRspObj.IsExist)
        {
            // 取得當前專案資訊，確認是否為其他專案的 Code
            var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
            };
            var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
            if (coEmpPrjDbGetRspObj?.EmployeeProjectCode != reqObj.EmployeeProjectCode)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.ProjectCodeDuplicate;
                return rspObj;
            }
        }

        // db, 核心-員工-專案-資料庫-更新
        var coEmpPrjDbUpdateReqObj = new CoEmpPrjDbUpdateReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectCode = reqObj.EmployeeProjectCode,
            EmployeeProjectName = reqObj.EmployeeProjectName,
        };
        var coEmpPrjDbUpdateRspObj = await this._coEmpProjectDb.UpdateAsync(coEmpPrjDbUpdateReqObj);
        if (coEmpPrjDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>恢復新增專案</summary>
    private async Task RecoveryAddProjectAsync(int employeeProjectID)
    {
        var reqObj = new
        {
            EmployeeProjectID = employeeProjectID,
        };

        // db, 核心-員工-專案-資料庫-移除
        var coEmpPrjDbRemoveReqObj = new CoEmpPrjDbRemoveReqMdl()
        {
            EmployeeProjectID = employeeProjectID,
        };
        var coEmpPrjDbRemoveRspObj = await this._coEmpProjectDb.RemoveAsync(coEmpPrjDbRemoveReqObj);
        if (coEmpPrjDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }

        // db, 核心-員工-專案契約-資料庫-移除多筆
        var coEmpPcDbRemoveReqObj = new CoEmpPcDbRemoveManyReqMdl()
        {
            EmployeeProjectID = employeeProjectID,
        };
        var coEmpPcDbRemoveRspObj = await this._coEmpProjectContractDb.RemoveManyAsync(coEmpPcDbRemoveReqObj);
        if (coEmpPcDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }
        // db, 核心-員工-專案工作計劃書-資料庫-移除多筆
        var coEmpPwDbRemoveReqObj = new CoEmpPwDbRemoveManyReqMdl()
        {
            EmployeeProjectID = employeeProjectID
        };
        var coEmpPwDbRemoveRspObj = await this._coEmpProjectWorkDb.RemoveManyAsync(coEmpPwDbRemoveReqObj);
        if (coEmpPwDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }
    }

    #endregion

    #region 合約

    /// <summary>管理者後台-工作-專案-邏輯服務-新增合約</summary>
    public async Task<MbsWrkPrjLgcAddContractRspMdl> AddContractAsync(MbsWrkPrjLgcAddContractReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcAddContractRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案契約-取得多筆ID
        var coEmpPcDbGetManyIdReqObj = new CoEmpPcDbGetManyIdReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPcDbGetManyIdRspObj = await this._coEmpProjectContractDb.GetManyIdAsync(coEmpPcDbGetManyIdReqObj);
        if (coEmpPcDbGetManyIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案契約-更新多筆
        var coEmpPcDbUpdateManyReqObj = new CoEmpPcDbUpdateManyReqMdl()
        {
            EmployeeProjectContractIdList = coEmpPcDbGetManyIdRspObj.EmployeeProjectContractList
                .Select(x => x.EmployeeProjectContractID)
                .Distinct()
                .ToList(),
            EmployeeProjectContractIsNewest = false,
        };
        var coEmpPcDbUpdateManyRspObj = await this._coEmpProjectContractDb.UpdateManyAsync(coEmpPcDbUpdateManyReqObj);
        if (coEmpPcDbUpdateManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案契約-新增
        var coEmpPcDbAddReqObj = new CoEmpPcDbAddReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectContractUrl = reqObj.EmployeeProjectContractUrl,
            EmployeeProjectContractIsNewest = true,
        };
        var coEmpPcDbAddRspObj = await this._coEmpProjectContractDb.AddAsync(coEmpPcDbAddReqObj);
        if (coEmpPcDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 工作計劃書

    /// <summary>管理者後台-工作-專案-邏輯服務-新增工作計劃書</summary>
    public async Task<MbsWrkPrjLgcAddWorkRspMdl> AddWorkAsync(MbsWrkPrjLgcAddWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcAddWorkRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案工作計劃書-取得多筆ID
        var coEmpPwDbGetManyIdReqObj = new CoEmpPwDbGetManyIdReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPwDbGetManyIdRspObj = await this._coEmpProjectWorkDb.GetManyIdAsync(coEmpPwDbGetManyIdReqObj);
        if (coEmpPwDbGetManyIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案工作計劃書-更新多筆
        var coEmpPwDbUpdateManyReqObj = new CoEmpPwDbUpdateManyReqMdl()
        {
            EmployeeProjectWorkIdList = coEmpPwDbGetManyIdRspObj.EmployeeProjectWorkList
                .Select(x => x.EmployeeProjectWorkID)
                .Distinct()
                .ToList(),
            EmployeeProjectWorkIsNewest = false,
        };
        var coEmpPwDbUpdateManyRspObj = await this._coEmpProjectWorkDb.UpdateManyAsync(coEmpPwDbUpdateManyReqObj);
        if (coEmpPwDbUpdateManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案工作計劃書-新增
        var coEmpPwDbAddReqObj = new CoEmpPwDbAddReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectWorkUrl = reqObj.EmployeeProjectWorkUrl,
            EmployeeProjectWorkIsNewest = true,
        };
        var coEmpPwDbAddRspObj = await this._coEmpProjectWorkDb.AddAsync(coEmpPwDbAddReqObj);
        if (coEmpPwDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 成員

    /// <summary>管理者後台-工作-專案-邏輯服務-新增成員</summary>
    public async Task<MbsWrkPrjLgcAddMemberRspMdl> AddMemberAsync(MbsWrkPrjLgcAddMemberReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcAddMemberRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案成員-資料庫-是否存在
        var coEmpPmDbExistReqObj = new CoEmpPmDbExistReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = reqObj.EmployeeID,
            EmployeeProjectMemberRole = reqObj.EmployeeProjectMemberRole,
        };
        var coEmpPmDbExistRspObj = await this._coEmpProjectMemberDb.ExistAsync(coEmpPmDbExistReqObj);
        if (coEmpPmDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPmDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-新增
        var coEmpPmDbAddReqObj = new CoEmpPmDbAddReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = reqObj.EmployeeID,
            EmployeeProjectMemberRole = reqObj.EmployeeProjectMemberRole,
        };
        var coEmpPmDbAddRspObj = await this._coEmpProjectMemberDb.AddAsync(coEmpPmDbAddReqObj);
        if (coEmpPmDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-移除成員</summary>
    public async Task<MbsWrkPrjLgcRemoveMemberRspMdl> RemoveMemberAsync(MbsWrkPrjLgcRemoveMemberReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcRemoveMemberRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得
        var coEmpPmDbGetReqObj = new CoEmpPmDbGetReqMdl()
        {
            EmployeeProjectMemberID = reqObj.EmployeeProjectMemberID,
        };
        var coEmpPmDbGetRspObj = await this._coEmpProjectMemberDb.GetAsync(coEmpPmDbGetReqObj);
        if (coEmpPmDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPmDbGetRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案成員-取得多筆 (該專案的所有成員)
        var coEmpPmDbGetManyAllReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPmDbGetRspObj.EmployeeProjectID,
        };
        var coEmpPmDbGetManyAllRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyAllReqObj);
        if (coEmpPmDbGetManyAllRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-移除
        var coEmpPmDbRemoveReqObj = new CoEmpPmDbRemoveReqMdl()
        {
            EmployeeProjectMemberID = reqObj.EmployeeProjectMemberID,
        };
        var coEmpPmDbRemoveRspObj = await this._coEmpProjectMemberDb.RemoveAsync(coEmpPmDbRemoveReqObj);
        if (coEmpPmDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆成員角色</summary>
    public async Task<MbsWrkPrjLgcGetManyMemberRoleRspMdl> GetManyMemberRoleAsync(MbsWrkPrjLgcGetManyMemberRoleReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetManyMemberRoleRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectMemberList = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Select(x => new MbsWrkPrjLgcGetManyMemberRoleRspItemMdl
            {
                EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 里程碑

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑</summary>
    public async Task<MbsWrkPrjLgcGetManyProjectStoneRspMdl> GetManyProjectStoneAsync(MbsWrkPrjLgcGetManyProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetManyProjectStoneRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑-取得多筆
        var coEmpPsDbGetManyReqObj = new CoEmpPsDbGetManyReqMdl()
        {
            EmployeeProjectIdList = new List<int>()
            {
                reqObj.EmployeeProjectID,
            },
        };
        var coEmpPsDbGetManyRspObj = await this._coEmpProjectStoneDb.GetManyAsync(coEmpPsDbGetManyReqObj);
        if (coEmpPsDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項-取得多筆
        var coEmpPsjDbGetManyReqObj = new CoEmpPsjDbGetManyReqMdl()
        {
            EmployeeProjectIdList = new List<int>()
            {
                reqObj.EmployeeProjectID,
            },
        };
        var coEmpPsjDbGetManyRspObj = await this._coEmpProjectStoneJobDb.GetManyAsync(coEmpPsjDbGetManyReqObj);
        if (coEmpPsjDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
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
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneList = coEmpPsDbGetManyRspObj.EmployeeProjectStoneList
            .Select(epsx => new MbsWrkPrjLgcGetManyProjectStoneRspItemStoneMdl
            {
                EmployeeProjectStoneID = epsx.EmployeeProjectStoneID,
                EmployeeProjectStoneName = epsx.EmployeeProjectStoneName,
                EmployeeProjectStoneStartTime = epsx.EmployeeProjectStoneStartTime,
                EmployeeProjectStoneEndTime = epsx.EmployeeProjectStoneEndTime,
                EmployeeProjectStonePreNotifyDay = epsx.EmployeeProjectStonePreNotifyDay,
                AtomEmployeeProjectStatus = epsx.AtomEmployeeProjectStatus,
                EmployeeProjectStoneJobList = coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList
                    .Where(epstx => epstx.EmployeeProjectStoneID == epsx.EmployeeProjectStoneID)
                    .Select(epstx => new MbsWrkPrjLgcGetManyProjectStoneRspItemTaskMdl
                    {
                        EmployeeProjectStoneJobID = epstx.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobName = epstx.EmployeeProjectStoneJobName,
                        EmployeeProjectStoneJobStartTime = epstx.EmployeeProjectStoneJobStartTime,
                        EmployeeProjectStoneJobEndTime = epstx.EmployeeProjectStoneJobEndTime,
                        AtomEmployeeProjectStatus = epstx.AtomEmployeeProjectStatus,
                        EmployeeProjectStoneJobExecutorCount = coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                            .Count(epstex => epstex.EmployeeProjectStoneJobID == epstx.EmployeeProjectStoneJobID),
                    })
                    .ToList(),
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑</summary>
    public async Task<MbsWrkPrjLgcGetProjectStoneRspMdl> GetProjectStoneAsync(MbsWrkPrjLgcGetProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetProjectStoneRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑-取得
        var coEmpPsDbGetReqObj = new CoEmpPsDbGetReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsDbGetRspObj = await this._coEmpProjectStoneDb.GetAsync(coEmpPsDbGetReqObj);
        if (coEmpPsDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項-取得多筆
        var coEmpPsjDbGetManyReqObj = new CoEmpPsjDbGetManyReqMdl()
        {
            EmployeeProjectStoneIdList = new List<int>()
            {
                reqObj.EmployeeProjectStoneID,
            },
        };
        var coEmpPsjDbGetManyRspObj = await this._coEmpProjectStoneJobDb.GetManyAsync(coEmpPsjDbGetManyReqObj);
        if (coEmpPsjDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項執行者-取得多筆
        var coEmpPsjeDbGetManyReqObj = new CoEmpPsjeDbGetManyReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsjeDbGetManyRspObj = await this._coEmpProjectStoneJobExecutorDb.GetManyAsync(coEmpPsjeDbGetManyReqObj);
        if (coEmpPsjeDbGetManyRspObj == default)
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneName = coEmpPsDbGetRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = coEmpPsDbGetRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = coEmpPsDbGetRspObj.EmployeeProjectStoneEndTime;
        rspObj.EmployeeProjectStonePreNotifyDay = coEmpPsDbGetRspObj.EmployeeProjectStonePreNotifyDay;
        rspObj.EmployeeProjectStoneJobList = coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList
            .Select(x => new MbsWrkPrjLgcGetProjectStoneRspItemJobMdl
            {
                EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName,
                EmployeeProjectStoneJobStartTime = x.EmployeeProjectStoneJobStartTime,
                EmployeeProjectStoneJobEndTime = x.EmployeeProjectStoneJobEndTime,
                EmployeeProjectStoneJobWorkHour = x.EmployeeProjectStoneJobWorkHour,
                EmployeeProjectStoneJobExecutorList =
                    (
                        from epste in coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                            .Where(epstex => epstex.EmployeeProjectStoneJobID == x.EmployeeProjectStoneJobID)
                        from e in coEmpInfDbGetManyNameRspObj.EmployeeList
                            .Where(ex => ex.EmployeeID == epste.EmployeeID)
                        select new MbsWrkPrjLgcGetProjectStoneRspItemExecutorMdl
                        {
                            EmployeeID = epste.EmployeeID,
                            EmployeeName = e.EmployeeName,
                        }
                    ).ToList(),
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-新增專案里程碑</summary>
    public async Task<MbsWrkPrjLgcAddProjectStoneRspMdl> AddProjectStoneAsync(MbsWrkPrjLgcAddProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcAddProjectStoneRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-取得
        var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
        if (coEmpPrjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷 里程碑時間 是否在 專案時間內
        if (reqObj.EmployeeProjectStoneStartTime < coEmpPrjDbGetRspObj.EmployeeProjectStartTime
            || reqObj.EmployeeProjectStoneEndTime > coEmpPrjDbGetRspObj.EmployeeProjectEndTime)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑-資料庫-新增
        var coEmpPsDbAddReqObj = new CoEmpPsDbAddReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneName = reqObj.EmployeeProjectStoneName,
            EmployeeProjectStoneStartTime = reqObj.EmployeeProjectStoneStartTime,
            EmployeeProjectStoneEndTime = reqObj.EmployeeProjectStoneEndTime,
            EmployeeProjectStonePreNotifyDay = reqObj.EmployeeProjectStonePreNotifyDay,
            AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.Undefined,
        };
        var coEmpPsDbAddRspObj = await this._coEmpProjectStoneDb.AddAsync(coEmpPsDbAddReqObj);
        if (coEmpPsDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // foreach, 里程碑工項列表
        var isSuccess = true;
        foreach (var employeeProjectStoneTaskItem in reqObj.EmployeeProjectStoneJobList)
        {
            // db, 核心-員工-專案里程碑工項-資料庫-新增
            var coEmpPsjDbAddReqObj = new CoEmpPsjDbAddReqMdl()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = coEmpPsDbAddRspObj.EmployeeProjectStoneID,
                EmployeeProjectStoneJobName = employeeProjectStoneTaskItem.EmployeeProjectStoneJobName,
                EmployeeProjectStoneJobStartTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobStartTime,
                EmployeeProjectStoneJobEndTime = employeeProjectStoneTaskItem.EmployeeProjectStoneJobEndTime,
                EmployeeProjectStoneJobWorkHour = employeeProjectStoneTaskItem.EmployeeProjectStoneJobWorkHour,
                AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.Undefined,
                EmployeeProjectStoneJobRemark = employeeProjectStoneTaskItem.EmployeeProjectStoneJobRemark,
            };
            var coEmpPsjDbAddRspObj = await this._coEmpProjectStoneJobDb.AddAsync(coEmpPsjDbAddReqObj);
            if (coEmpPsjDbAddRspObj == default)
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", item: {JsonSerializer.Serialize(employeeProjectStoneTaskItem)}");

                isSuccess = false;
                break;
            }

            // db, 核心-員工-專案里程碑工項清單-資料庫-新增
            var coEmpPsjbDbAddReqObj = new CoEmpPsjbDbAddReqMdl()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = coEmpPsDbAddRspObj.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = coEmpPsjDbAddRspObj.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobBucketName = DbEmployeeProjectStoneJobBucket.Default.Name,
                EmployeeProjectStoneJobBucketIsFinish = DbEmployeeProjectStoneJobBucket.Default.IsFinish,
            };
            var coEmpPsjbDbAddRspObj = await this._coEmpProjectStoneJobBucketDb.AddAsync(coEmpPsjbDbAddReqObj);
            if (coEmpPsjbDbAddRspObj == default)
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", item: {JsonSerializer.Serialize(employeeProjectStoneTaskItem)}");
                isSuccess = false;
                break;
            }

            // db, 核心-員工-專案里程碑工項執行者-資料庫-新增多筆
            var coEmpPsjeDbAddManyReqObj = new CoEmpPsjeDbAddManyReqMdl()
            {
                EmployeeProjectStoneJobExecutoList = employeeProjectStoneTaskItem.EmployeeProjectStoneJobExecutorList
                    .Select(x => new CoEmpPsjeDbAddManyReqItemMdl
                    {
                        EmployeeProjectID = reqObj.EmployeeProjectID,
                        EmployeeProjectStoneID = coEmpPsDbAddRspObj.EmployeeProjectStoneID,
                        EmployeeProjectStoneJobID = coEmpPsjDbAddRspObj.EmployeeProjectStoneJobID,
                        EmployeeID = x.EmployeeID,
                    })
                    .ToList(),
            };
            var coEmpPsjeDbAddManyRspObj = await this._coEmpProjectStoneJobExecutorDb.AddManyAsync(coEmpPsjeDbAddManyReqObj);
            if (coEmpPsjeDbAddManyRspObj == default)
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", item: {JsonSerializer.Serialize(employeeProjectStoneTaskItem)}");

                isSuccess = false;
                break;
            }
        }

        //  判斷是否成功
        if (isSuccess == false)
        {
            // 失敗，清除新增資料，回傳失敗
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // this, 回復-新增專案里程碑
            await this.RecoveryAddProjectStoneAsync(coEmpPsDbAddRspObj.EmployeeProjectStoneID);

            return rspObj;
        }

        // logical, 檢查與更新專案狀態
        var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>回復-新增專案里程碑</summary>
    private async Task RecoveryAddProjectStoneAsync(int employeeProjectStoneID)
    {
        var reqObj = new
        {
            EmployeeProjectStoneID = employeeProjectStoneID,
        };

        // db, 核心-員工-專案里程碑-資料庫-移除
        var coEmpPsDbRemoveReqObj = new CoEmpPsDbRemoveReqMdl()
        {
            EmployeeProjectStoneID = employeeProjectStoneID,
        };
        var coEmpPsDbRemoveRspObj = await this._coEmpProjectStoneDb.RemoveAsync(coEmpPsDbRemoveReqObj);
        if (coEmpPsDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }

        // db, 核心-員工-專案里程碑工項-資料庫-移除多筆
        var coEmpPsjDbRemoveManyReqObj = new CoEmpPsjDbRemoveManyReqMdl()
        {
            EmployeeProjectStoneID = employeeProjectStoneID,
        };
        var coEmpPsjDbRemoveManyRspObj = await this._coEmpProjectStoneJobDb.RemoveManyAsync(coEmpPsjDbRemoveManyReqObj);
        if (coEmpPsjDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }

        // db, 核心-員工-專案里程碑工項清單-資料庫-移除多筆
        var coEmpPsjbDbRemoveManyReqObj = new CoEmpPsjbDbRemoveManyReqMdl()
        {
            EmployeeProjectStoneID = employeeProjectStoneID,
        };
        var coEmpPsjbDbRemoveManyRspObj = await this._coEmpProjectStoneJobBucketDb.RemoveManyAsync(coEmpPsjbDbRemoveManyReqObj);
        if (coEmpPsjbDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }

        // db, 核心-員工-專案里程碑工項執行者-資料庫-移除多筆
        var coEmpPsjeDbRemoveManyReqObj = new CoEmpPsjeDbRemoveManyReqMdl()
        {
            EmployeeProjectStoneID = employeeProjectStoneID,
        };
        var coEmpPsjeDbRemoveManyRspObj = await this._coEmpProjectStoneJobExecutorDb.RemoveManyAsync(coEmpPsjeDbRemoveManyReqObj);
        if (coEmpPsjeDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
        }

        return;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑</summary>
    public async Task<MbsWrkPrjLgcUpdateProjectStoneRspMdl> UpdateProjectStoneAsync(MbsWrkPrjLgcUpdateProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcUpdateProjectStoneRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑-取得員工專案ID
        var coEmpPsDbGetEmployeeProjectIdReqObj = new CoEmpPsDbGetEmployeeProjectIdReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsDbGetEmployeeProjectIdRspObj = await this._coEmpProjectStoneDb.GetEmployeeProjectIdAsync(coEmpPsDbGetEmployeeProjectIdReqObj);
        if (coEmpPsDbGetEmployeeProjectIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項-取得多筆ID
        var coEmpPsjDbGetManyIdReqObj = new CoEmpPsjDbGetManyIdReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsjDbGetManyIdRspObj = await this._coEmpProjectStoneJobDb.GetManyIdAsync(coEmpPsjDbGetManyIdReqObj);
        if (coEmpPsjDbGetManyIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑-資料庫-更新
        var coEmpPsDbUpdateReqObj = new CoEmpPsDbUpdateReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneName = reqObj.EmployeeProjectStoneName,
            EmployeeProjectStoneStartTime = reqObj.EmployeeProjectStoneStartTime,
            EmployeeProjectStoneEndTime = reqObj.EmployeeProjectStoneEndTime,
            EmployeeProjectStonePreNotifyDay = reqObj.EmployeeProjectStonePreNotifyDay,
            AtomEmployeeProjectStatus = null,
        };
        var coEmpPsDbUpdateRspObj = await this._coEmpProjectStoneDb.UpdateAsync(coEmpPsDbUpdateReqObj);
        if (coEmpPsDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 處理專案里程碑工項
        if (reqObj.EmployeeProjectStoneJobList != null)
        {
            // 取得該移除的專案里程碑工項ID列表
            var removeEmployeeProjectStoneJobIdList =
                (
                    from db in coEmpPsjDbGetManyIdRspObj.EmployeeProjectStoneJobList
                    from req in reqObj.EmployeeProjectStoneJobList
                        .Where(reqx => reqx.EmployeeProjectStoneJobID == db.EmployeeProjectStoneJobID)
                        .DefaultIfEmpty()
                    where req == null
                    select db.EmployeeProjectStoneJobID
                ).Distinct()
                .ToList();

            // 取得該更新的專案里程碑工項列表
            var updateEmployeeProjectStoneJobList = reqObj.EmployeeProjectStoneJobList
                .Where(x => x.EmployeeProjectStoneJobID > 0)
                .ToList();

            // 取得該新增的專案里程碑工項列表
            var addEmployeeProjectStoneJobList = reqObj.EmployeeProjectStoneJobList
                .Where(x => x.EmployeeProjectStoneJobID < 0)
                .ToList();

            // 移除工項
            // db, 核心-員工-專案里程碑工項-資料庫-移除多筆
            var coEmpPsjDbRemoveManyReqObj = new CoEmpPsjDbRemoveManyReqMdl()
            {
                EmployeeProjectStoneJobIdList = removeEmployeeProjectStoneJobIdList,
            };
            var coEmpPsjDbRemoveManyRspObj = await this._coEmpProjectStoneJobDb.RemoveManyAsync(coEmpPsjDbRemoveManyReqObj);
            if (coEmpPsjDbRemoveManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 更新工項
            // foreach, db, 核心-員工-專案里程碑工項-資料庫-更新
            foreach (var updateEmployeeProjectStoneJobItem in updateEmployeeProjectStoneJobList)
            {
                // db, 核心-員工-專案里程碑工項-資料庫-更新
                var coEmpPsjDbUpdateReqObj = new CoEmpPsjDbUpdateReqMdl()
                {
                    EmployeeProjectStoneJobID = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobName = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobName,
                    EmployeeProjectStoneJobStartTime = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobWorkHour,
                };
                var coEmpPsjDbUpdateRspObj = await this._coEmpProjectStoneJobDb.UpdateAsync(coEmpPsjDbUpdateReqObj);
                if (coEmpPsjDbUpdateRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(updateEmployeeProjectStoneJobItem)}");
                    return rspObj;
                }

                // db, 核心-員工-專案里程碑工項執行者-資料庫-移除多筆
                var coEmpPsjeDbRemoveManyReqObj = new CoEmpPsjeDbRemoveManyReqMdl()
                {
                    EmployeeProjectStoneJobID = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobID,
                };
                var coEmpPsjeDbRemoveManyRspObj = await this._coEmpProjectStoneJobExecutorDb.RemoveManyAsync(coEmpPsjeDbRemoveManyReqObj);
                if (coEmpPsjeDbRemoveManyRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(updateEmployeeProjectStoneJobItem)}");
                    return rspObj;
                }

                // db, 核心-員工-專案里程碑工項執行者-資料庫-新增多筆
                var coEmpPsjeDbAddManyReqObj = new CoEmpPsjeDbAddManyReqMdl()
                {
                    EmployeeProjectStoneJobExecutoList = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobExecutorList
                        .Select(x => new CoEmpPsjeDbAddManyReqItemMdl
                        {
                            EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
                            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                            EmployeeProjectStoneJobID = updateEmployeeProjectStoneJobItem.EmployeeProjectStoneJobID,
                            EmployeeID = x.EmployeeID,
                        })
                        .ToList(),
                };
                var coEmpPsjeDbAddManyRspObj = await this._coEmpProjectStoneJobExecutorDb.AddManyAsync(coEmpPsjeDbAddManyReqObj);
                if (coEmpPsjeDbAddManyRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(updateEmployeeProjectStoneJobItem)}");
                    return rspObj;
                }
            }

            // 新增工項
            foreach (var addEmployeeProjectStoneJobItem in addEmployeeProjectStoneJobList)
            {
                // db, 核心-員工-專案里程碑工項-資料庫-新增
                var coEmpPsjDbAddReqObj = new CoEmpPsjDbAddReqMdl()
                {
                    EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
                    EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobName = addEmployeeProjectStoneJobItem.EmployeeProjectStoneJobName,
                    EmployeeProjectStoneJobStartTime = addEmployeeProjectStoneJobItem.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = addEmployeeProjectStoneJobItem.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = addEmployeeProjectStoneJobItem.EmployeeProjectStoneJobWorkHour,
                    AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.NotAssigned,
                };
                var coEmpPsjDbAddRspObj = await this._coEmpProjectStoneJobDb.AddAsync(coEmpPsjDbAddReqObj);
                if (coEmpPsjDbAddRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(addEmployeeProjectStoneJobItem)}");
                    return rspObj;
                }

                // db, 核心-員工-專案里程碑工項清單-資料庫-新增
                var coEmpPsjbDbAddReqObj = new CoEmpPsjbDbAddReqMdl()
                {
                    EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
                    EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobID = coEmpPsjDbAddRspObj.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobBucketName = DbEmployeeProjectStoneJobBucket.Default.Name,
                    EmployeeProjectStoneJobBucketIsFinish = DbEmployeeProjectStoneJobBucket.Default.IsFinish,
                };
                var coEmpPsjbDbAddRspObj = await this._coEmpProjectStoneJobBucketDb.AddAsync(coEmpPsjbDbAddReqObj);
                if (coEmpPsjbDbAddRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(addEmployeeProjectStoneJobItem)}");
                    return rspObj;
                }

                // db, 核心-員工-專案里程碑工項執行者-資料庫-新增多筆
                var coEmpPsjeDbAddManyReqObj = new CoEmpPsjeDbAddManyReqMdl()
                {
                    EmployeeProjectStoneJobExecutoList = addEmployeeProjectStoneJobItem.EmployeeProjectStoneJobExecutorList
                        .Select(x => new CoEmpPsjeDbAddManyReqItemMdl
                        {
                            EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
                            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                            EmployeeProjectStoneJobID = coEmpPsjDbAddRspObj.EmployeeProjectStoneJobID,
                            EmployeeID = x.EmployeeID,
                        })
                        .ToList(),
                };
                var coEmpPsjeDbAddManyRspObj = await this._coEmpProjectStoneJobExecutorDb.AddManyAsync(coEmpPsjeDbAddManyReqObj);
                if (coEmpPsjeDbAddManyRspObj == default)
                {
                    this._logger.LogError(
                        $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                        $", item: {JsonSerializer.Serialize(addEmployeeProjectStoneJobItem)}");
                    return rspObj;
                }

            }

        }

        // logical, 檢查與更新專案狀態
        var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
        {
            EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
        };
        await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-移除專案里程碑</summary>
    public async Task<MbsWrkPrjLgcRemoveProjectStoneRspMdl> RemoveProjectStoneAsync(MbsWrkPrjLgcRemoveProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcRemoveProjectStoneRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑-取得員工專案ID
        var coEmpPsDbGetEmployeeProjectIdReqObj = new CoEmpPsDbGetEmployeeProjectIdReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsDbGetEmployeeProjectIdRspObj = await this._coEmpProjectStoneDb.GetEmployeeProjectIdAsync(coEmpPsDbGetEmployeeProjectIdReqObj);
        if (coEmpPsDbGetEmployeeProjectIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑-資料庫-移除
        var coEmpPsDbRemoveReqObj = new CoEmpPsDbRemoveReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsDbRemoveRspObj = await this._coEmpProjectStoneDb.RemoveAsync(coEmpPsDbRemoveReqObj);
        if (coEmpPsDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項-資料庫-移除多筆
        var coEmpPsjDbRemoveManyReqObj = new CoEmpPsjDbRemoveManyReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsjDbRemoveManyRspObj = await this._coEmpProjectStoneJobDb.RemoveManyAsync(coEmpPsjDbRemoveManyReqObj);
        if (coEmpPsjDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項清單-資料庫-移除多筆
        var coEmpPsjbDbRemoveManyReqObj = new CoEmpPsjbDbRemoveManyReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsjbDbRemoveManyRspObj = await this._coEmpProjectStoneJobBucketDb.RemoveManyAsync(coEmpPsjbDbRemoveManyReqObj);
        if (coEmpPsjbDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項執行者-資料庫-移除多筆
        var coEmpPsjeDbRemoveManyReqObj = new CoEmpPsjeDbRemoveManyReqMdl()
        {
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var coEmpPsjeDbRemoveManyRspObj = await this._coEmpProjectStoneJobExecutorDb.RemoveManyAsync(coEmpPsjeDbRemoveManyReqObj);
        if (coEmpPsjeDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // logical, 檢查與更新專案狀態
        var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
        {
            EmployeeProjectID = coEmpPsDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
        };
        await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 工項

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項</summary>
    public async Task<MbsWrkPrjLgcGetManyProjectStoneJobRspMdl> GetManyProjectStoneJobAsync(MbsWrkPrjLgcGetManyProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetManyProjectStoneJobRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.GeneralManager,
            DbEmployeeProjectMemberRoleEnum.Saler,
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
            DbEmployeeProjectMemberRoleEnum.Executor,
            DbEmployeeProjectMemberRoleEnum.Assistant
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-取得
        var coEmpPmDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPmDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPmDbGetReqObj);
        if (coEmpPmDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑-取得多筆
        var coEmpPsDbGetManyReqObj = new CoEmpPsDbGetManyReqMdl()
        {
            EmployeeProjectIdList = new List<int>()
            {
                reqObj.EmployeeProjectID,
            },
        };
        var coEmpPsDbGetManyRspObj = await this._coEmpProjectStoneDb.GetManyAsync(coEmpPsDbGetManyReqObj);
        if (coEmpPsDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項-取得多筆從里程碑ID列表
        var coEmpPsjDbGetManyReqObj = new CoEmpPsjDbGetManyReqMdl()
        {
            EmployeeProjectStoneIdList = coEmpPsDbGetManyRspObj.EmployeeProjectStoneList
                .Select(x => x.EmployeeProjectStoneID)
                .Distinct()
                .ToList(),
        };
        var coEmpPsjDbGetManyRspObj = await this._coEmpProjectStoneJobDb.GetManyAsync(coEmpPsjDbGetManyReqObj);
        if (coEmpPsjDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項清單-資料庫-取得多筆從工項ID列表
        var coEmpPsjbDbGetManyFromTaskIdListReqObj = new CoEmpPsjbDbGetManyFromTaskIdListReqMdl()
        {
            EmployeeProjectStoneJobIDList = coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList
                .Select(x => x.EmployeeProjectStoneJobID)
                .Distinct()
                .ToList(),
        };
        var coEmpPsjbDbGetManyFromTaskIdListRspObj = await this._coEmpProjectStoneJobBucketDb.GetManyFromTaskIdListAsync(coEmpPsjbDbGetManyFromTaskIdListReqObj);
        if (coEmpPsjbDbGetManyFromTaskIdListRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項執行者-取得多筆數量從工項ID列表
        var coEmpPsjeDbGetManyCountFromTaskIdListReqObj = new CoEmpPsjeDbGetManyCountFromTaskIdListReqMdl()
        {
            EmployeeProjectStoneJobIDList = coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList
                .Select(x => x.EmployeeProjectStoneJobID)
                .Distinct()
                .ToList(),
        };
        var coEmpPsjeDbGetManyCountFromTaskIdListRspObj = await this._coEmpProjectStoneJobExecutorDb.GetManyCountFromTaskIdListAsync(coEmpPsjeDbGetManyCountFromTaskIdListReqObj);
        if (coEmpPsjeDbGetManyCountFromTaskIdListRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneList = (
            from stone in coEmpPsDbGetManyRspObj.EmployeeProjectStoneList
            select new MbsWrkPrjLgcGetManyProjectStoneJobRspItemStoneMdl()
            {
                EmployeeProjectStoneID = stone.EmployeeProjectStoneID,
                EmployeeProjectStoneName = stone.EmployeeProjectStoneName,
                EmployeeProjectStonePreNotifyDay = stone.EmployeeProjectStonePreNotifyDay,
                EmployeeProjectStoneStartTime = stone.EmployeeProjectStoneStartTime,
                EmployeeProjectStoneEndTime = stone.EmployeeProjectStoneEndTime,
                AtomEmployeeProjectStatus = stone.AtomEmployeeProjectStatus,
                EmployeeProjectStoneJobList = (
                    from task in coEmpPsjDbGetManyRspObj.EmployeeProjectStoneJobList
                        .Where(task => task.EmployeeProjectStoneID == stone.EmployeeProjectStoneID)
                    from executorCount in coEmpPsjeDbGetManyCountFromTaskIdListRspObj.EmployeeProjectStoneJobExecutorCountList
                        .Where(count => count.EmployeeProjectStoneJobID == task.EmployeeProjectStoneJobID)
                        .DefaultIfEmpty()
                    select new MbsWrkPrjLgcGetManyProjectStoneJobRspItemJobMdl()
                    {
                        EmployeeProjectStoneJobID = task.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobName = task.EmployeeProjectStoneJobName,
                        EmployeeProjectStoneJobStartTime = task.EmployeeProjectStoneJobStartTime,
                        EmployeeProjectStoneJobEndTime = task.EmployeeProjectStoneJobEndTime,
                        EmployeeProjectStoneJobWorkHour = task.EmployeeProjectStoneJobWorkHour,
                        AtomEmployeeProjectStatus = task.AtomEmployeeProjectStatus,
                        EmployeeProjectStoneJobExecutorCount = executorCount?.ExecutorCount ?? 0,
                        EmployeeProjectStoneJobBucketList = (
                            from bucket in coEmpPsjbDbGetManyFromTaskIdListRspObj.EmployeeProjectStoneJobBucketList
                                .Where(bucket => bucket.EmployeeProjectStoneJobID == task.EmployeeProjectStoneJobID)
                            select new MbsWrkPrjLgcGetManyProjectStoneJobRspItemBucketMdl()
                            {
                                EmployeeProjectStoneJobBucketID = bucket.EmployeeProjectStoneJobBucketID,
                                EmployeeProjectStoneJobBucketName = bucket.EmployeeProjectStoneJobBucketName,
                                EmployeeProjectStoneJobBucketIsFinish = bucket.EmployeeProjectStoneJobBucketIsFinish,
                            }
                        ).ToList()
                    }
                ).ToList()
            }
        ).ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑工項</summary>
    public async Task<MbsWrkPrjLgcGetProjectStoneJobRspMdl> GetProjectStoneJobAsync(MbsWrkPrjLgcGetProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetProjectStoneJobRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.GeneralManager,
            DbEmployeeProjectMemberRoleEnum.Saler,
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
            DbEmployeeProjectMemberRoleEnum.Executor,
            DbEmployeeProjectMemberRoleEnum.Assistant
        };

        // db, 核心-員工-專案里程碑工項-取得
        var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl()
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID
        };
        var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
        if (coEmpPsjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

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

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfDbGetManyNameReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                        .Select(x => x.EmployeeID)
                        .Distinct()
                        .ToList()
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneID = coEmpPsjDbGetRspObj.EmployeeProjectStoneID;
        rspObj.EmployeeProjectStoneName = coEmpPsDbGetRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = coEmpPsDbGetRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = coEmpPsDbGetRspObj.EmployeeProjectStoneEndTime;
        rspObj.EmployeeProjectStoneJobName = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobName;
        rspObj.EmployeeProjectStoneJobStartTime = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobStartTime;
        rspObj.EmployeeProjectStoneJobEndTime = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobEndTime;
        rspObj.EmployeeProjectStoneJobWorkHour = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobWorkHour;
        rspObj.AtomEmployeeProjectStatus = coEmpPsjDbGetRspObj.AtomEmployeeProjectStatus;
        rspObj.EmployeeProjectStoneJobRemark = coEmpPsjDbGetRspObj.EmployeeProjectStoneJobRemark;
        rspObj.EmployeeProjectStoneJobExecutorList =
            (
                from excutor in coEmpPsjeDbGetManyRspObj.EmployeeProjectStoneJobExecutorList
                from e in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(x => x.EmployeeID == excutor.EmployeeID)
                select new MbsWrkPrjLgcGetProjectStoneJobRspItmeExecutorMdl
                {
                    EmployeeProjectStoneJobExecutorEmployeeID = excutor.EmployeeID,
                    EmployeeProjectStoneJobExecutorEmployeeName = e.EmployeeName
                }
            ).ToList();
        rspObj.EmployeeProjectStoneJobBucketList = coEmpPsjbDbGetManyRspObj.EmployeeProjectStoneJobBucketList
            .Select(x => new MbsWrkPrjLgcGetProjectStoneJobRspItmeBucketMdl
            {
                EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish
            }).ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑工項</summary>
    public async Task<MbsWrkPrjLgcUpdateProjectStoneJobRspMdl> UpdateProjectStoneJobAsync(MbsWrkPrjLgcUpdateProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcUpdateProjectStoneJobRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
            DbEmployeeProjectMemberRoleEnum.Executor,
            DbEmployeeProjectMemberRoleEnum.Assistant
        };

        // db, 核心-員工-專案里程碑工項-取得
        var coEmpPsjDbGetReqObj = new CoEmpPsjDbGetReqMdl()
        {
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID
        };
        var coEmpPsjDbGetRspObj = await this._coEmpProjectStoneJobDb.GetAsync(coEmpPsjDbGetReqObj);
        if (coEmpPsjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 檢查清單，至少要有一個清單
        if (reqObj.EmployeeProjectStoneJobExecutorIdList != null
            && reqObj.EmployeeProjectStoneJobExecutorIdList.Count == 0)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查執行者ID是否存在
        if (reqObj.EmployeeProjectStoneJobExecutorIdList != null &&
            reqObj.EmployeeProjectStoneJobExecutorIdList.Count > 0)
        {
            // db, 核心-員工-資訊-資料庫-檢查多筆存在
            var coEmpInfDbCheckManyExistReqObj = new CoEmpInfDbCheckManyExistReqMdl()
            {
                EmployeeIdList = reqObj.EmployeeProjectStoneJobExecutorIdList
            };
            var coEmpInfDbCheckManyExistRspObj = await this._coEmpInfoDb.CheckManyExistAsync(coEmpInfDbCheckManyExistReqObj);
            if (coEmpInfDbCheckManyExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            if (coEmpInfDbCheckManyExistRspObj.EmployeeList.Any(x => x.IsExist == false))
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-員工-專案里程碑工項-更新
        var coEmpPsjDbUpdateReqObj = new CoEmpPsjDbUpdateReqMdl()
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

        // 更新執行者ID(批次刪除,批次新增)
        if (reqObj.EmployeeProjectStoneJobExecutorIdList != null &&
            reqObj.EmployeeProjectStoneJobExecutorIdList.Count > 0)
        {
            // db, 核心-員工-專案里程碑工項執行者-資料庫-移除多筆
            var coEmpPsjeDbRemoveManyReqObj = new CoEmpPsjeDbRemoveManyReqMdl()
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID
            };
            var coEmpPsjeDbRemoveManyRspObj = await this._coEmpProjectStoneJobExecutorDb.RemoveManyAsync(coEmpPsjeDbRemoveManyReqObj);
            if (coEmpPsjeDbRemoveManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-專案里程碑工項執行者-資料庫-新增多筆
            var coEmpPsjeDbAddManyReqObj = new CoEmpPsjeDbAddManyReqMdl()
            {
                EmployeeProjectStoneJobExecutoList = reqObj.EmployeeProjectStoneJobExecutorIdList
                .Select(x => new CoEmpPsjeDbAddManyReqItemMdl
                {
                    EmployeeID = x,
                    EmployeeProjectID = coEmpPsjDbGetRspObj.EmployeeProjectID,
                    EmployeeProjectStoneID = coEmpPsjDbGetRspObj.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID
                })
                .ToList()
            };
            var coEmpPsjeDbAddManyRspObj = await this._coEmpProjectStoneJobExecutorDb.AddManyAsync(coEmpPsjeDbAddManyReqObj);
            if (coEmpPsjeDbAddManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 更新工項清單(工項清單-ID, >0:更新, -1:新增)
        if (reqObj.EmployeeProjectStoneJobBucketList != null &&
            reqObj.EmployeeProjectStoneJobBucketList.Count > 0)
        {
            // db, 核心-員工-專案里程碑工項清單-資料庫-取得多筆
            var coEmpPsjbDbGetManyReqObj = new CoEmpPsjbDbGetManyReqMdl()
            {
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID
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
            if (deleteBucketList != null && deleteBucketList.Count > 0)
            {
                // db, 核心-員工-專案里程碑工項清單-資料庫-移除多筆
                var coEmpPsjbDbRemoveManyReqObj = new CoEmpPsjbDbRemoveManyReqMdl()
                {
                    EmployeeProjectStoneJobBucketIdList = deleteBucketList
                        .Select(x => x.EmployeeProjectStoneJobBucketID)
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
            foreach (var updateBucket in updateBucketList)
            {
                // db, 核心-員工-專案里程碑工項清單-資料庫-更新
                var coEmpPsjbDbUpdateReqObj = new CoEmpPsjbDbUpdateReqMdl()
                {
                    EmployeeProjectStoneJobBucketID = updateBucket.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketName = updateBucket.EmployeeProjectStoneJobBucketName,
                    EmployeeProjectStoneJobBucketIsFinish = updateBucket.EmployeeProjectStoneJobBucketIsFinish
                };
                var coEmpPsjbDbUpdateRspObj = await this._coEmpProjectStoneJobBucketDb.UpdateAsync(coEmpPsjbDbUpdateReqObj);
                if (coEmpPsjbDbUpdateRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
            }

            // 取得該新增的專案里程碑工項清單列表
            var addBucketList = reqObj.EmployeeProjectStoneJobBucketList
                .Where(x => x.EmployeeProjectStoneJobBucketID < 0)
                .ToList();
            if (addBucketList != null && addBucketList.Count > 0)
            {
                // db, 核心-員工-專案里程碑工項清單-資料庫-新增多筆
                var coEmpPsjbDbAddManyReqObj = new CoEmpPsjbDbAddManyReqMdl()
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

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑工項清單</summary>
    public async Task<MbsWrkPrjLgcUpdateProjectStoneJobBucketRspMdl> UpdateProjectStoneJobBucketAsync(MbsWrkPrjLgcUpdateProjectStoneJobBucketReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcUpdateProjectStoneJobBucketRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
            DbEmployeeProjectMemberRoleEnum.ProjectManager,
            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
            DbEmployeeProjectMemberRoleEnum.Executor,
            DbEmployeeProjectMemberRoleEnum.Assistant
        };

        // db, 核心-員工-專案里程碑工項清單-資料庫-取得
        var coEmpPsjbDbGetReqObj = new CoEmpPsjbDbGetReqMdl()
        {
            EmployeeProjectStoneJobBucketID = reqObj.EmployeeProjectStoneJobBucketID
        };
        var coEmpPsjbDbGetRspObj = await this._coEmpProjectStoneJobBucketDb.GetAsync(coEmpPsjbDbGetReqObj);
        if (coEmpPsjbDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPsjbDbGetRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案里程碑工項清單-資料庫-取得[員工專案ID]
        var coEmpPsjbDbGetEmployeeProjectIdReqObj = new CoEmpPsjbDbGetEmployeeProjectIdReqMdl()
        {
            EmployeeProjectStoneJobBucketID = reqObj.EmployeeProjectStoneJobBucketID,
        };
        var coEmpPsjbDbGetEmployeeProjectIdRspObj = await this._coEmpProjectStoneJobBucketDb.GetEmployeeProjectIdAsync(coEmpPsjbDbGetEmployeeProjectIdReqObj);
        if (coEmpPsjbDbGetEmployeeProjectIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項清單-資料庫-更新
        var coEmpPsjbDbUpdateReqObj = new CoEmpPsjbDbUpdateReqMdl()
        {
            EmployeeProjectStoneJobBucketID = reqObj.EmployeeProjectStoneJobBucketID,
            EmployeeProjectStoneJobBucketIsFinish = reqObj.EmployeeProjectStoneJobBucketIsFinish,
        };
        var coEmpPsjbDbUpdateRspObj = await this._coEmpProjectStoneJobBucketDb.UpdateAsync(coEmpPsjbDbUpdateReqObj);
        if (coEmpPsjbDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // logical, 檢查與更新專案狀態
        var mbsCmnLgcCheckAndUpdateProjectStatusReqObj = new MbsCmnLgcCheckAndUpdateProjectStatusReqMdl()
        {
            EmployeeProjectID = coEmpPsjbDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
        };
        await this._mbsCommonLogical.CheckAndUpdateProjectStatusAsync(mbsCmnLgcCheckAndUpdateProjectStatusReqObj);

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 支出

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案支出</summary>
    public async Task<MbsWrkPrjLgcGetExpenseRspMdl> GetExpenseAsync(MbsWrkPrjLgcGetExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetExpenseRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案支出-資料庫-取得員工專案ID
        var coEmpPrjExpDbGetEmployeeProjectIdReqObj = new CoEmpPrjExpDbGetEmployeeProjectIdReqMdl()
        {
            EmployeeProjectExpenseID = reqObj.EmployeeProjectExpenseID,
        };
        var coEmpPrjExpDbGetEmployeeProjectIdRspObj = await this._coEmpProjectExpenseDb.GetEmployeeProjectIdAsync(coEmpPrjExpDbGetEmployeeProjectIdReqObj);
        if (coEmpPrjExpDbGetEmployeeProjectIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
          DbEmployeeProjectMemberRoleEnum.Saler,
          DbEmployeeProjectMemberRoleEnum.ProjectManager,
          DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPrjExpDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案支出-資料庫-取得
        var coEmpPrjExpDbGetReqObj = new CoEmpPrjExpDbGetReqMdl()
        {
            EmployeeProjectExpenseID = reqObj.EmployeeProjectExpenseID,
        };
        var coEmpPrjExpDbGetRspObj = await this._coEmpProjectExpenseDb.GetAsync(coEmpPrjExpDbGetReqObj);
        if (coEmpPrjExpDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectID = coEmpPrjExpDbGetRspObj.EmployeeProjectID;
        rspObj.EmployeeProjectExpenseName = coEmpPrjExpDbGetRspObj.EmployeeProjectExpenseName;
        rspObj.EmployeeProjectExpensePredictAmount = coEmpPrjExpDbGetRspObj.EmployeeProjectExpensePredictAmount;
        rspObj.EmployeeProjectExpenseActualAmount = coEmpPrjExpDbGetRspObj.EmployeeProjectExpenseActualAmount;
        rspObj.EmployeeProjectExpenseBillNumber = coEmpPrjExpDbGetRspObj.EmployeeProjectExpenseBillNumber;
        rspObj.EmployeeProjectExpenseBillTime = coEmpPrjExpDbGetRspObj.EmployeeProjectExpenseBillTime;
        rspObj.EmployeeProjectExpenseRemark = coEmpPrjExpDbGetRspObj.EmployeeProjectExpenseRemark;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案支出</summary>
    public async Task<MbsWrkPrjLgcGetManyExpenseRspMdl> GetManyExpenseAsync(MbsWrkPrjLgcGetManyExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcGetManyExpenseRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
      {
          DbEmployeeProjectMemberRoleEnum.GeneralManager,
          DbEmployeeProjectMemberRoleEnum.Saler,
          DbEmployeeProjectMemberRoleEnum.ProjectManager,
          DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
          DbEmployeeProjectMemberRoleEnum.Executor,
          DbEmployeeProjectMemberRoleEnum.Assistant,
      };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案-資料庫-取得
        var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
        if (coEmpPrjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // http, EipV1-Http-PMS-查詢專案費用
        var eipV1HttpPmsGetProjectExpensesReqObj = new EipV1HttpPmsGetProjectExpensesReqMdl
        {
            ProjectCode = coEmpPrjDbGetRspObj.EmployeeProjectCode,
            ProjectName = coEmpPrjDbGetRspObj.EmployeeProjectName,
            StartDate = coEmpPrjDbGetRspObj.EmployeeProjectStartTime,
            EndDate = coEmpPrjDbGetRspObj.EmployeeProjectEndTime
        };
        var eipV1HttpPmsGetProjectExpensesRspObj = await this._eipV1Http.PmsGetProjectExpenseAsync(eipV1HttpPmsGetProjectExpensesReqObj);
        if (eipV1HttpPmsGetProjectExpensesRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // http, EipV1-Http-PMS-查詢專案旅費
        var eipV1HttpPmsGetProjectTravelExpenseReqObj = new EipV1HttpPmsGetProjectTravelExpenseReqMdl
        {
            ProjectCode = coEmpPrjDbGetRspObj.EmployeeProjectCode,
            ProjectName = coEmpPrjDbGetRspObj.EmployeeProjectName,
            StartDate = coEmpPrjDbGetRspObj.EmployeeProjectStartTime,
            EndDate = coEmpPrjDbGetRspObj.EmployeeProjectEndTime
        };
        var eipV1HttpPmsGetProjectTravelExpenseRspObj = await this._eipV1Http.PmsGetProjectTravelExpenseAsync(eipV1HttpPmsGetProjectTravelExpenseReqObj);
        if (eipV1HttpPmsGetProjectTravelExpenseRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案支出-資料庫-取得多筆
        var coEmpPrjExpDbGetManyReqObj = new CoEmpPrjExpDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var coEmpPrjExpDbGetManyRspObj = await this._coEmpProjectExpenseDb.GetManyAsync(coEmpPrjExpDbGetManyReqObj);
        if (coEmpPrjExpDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectExpenseList = coEmpPrjExpDbGetManyRspObj.EmployeeProjectExpenseList
            .Select(x => new MbsWrkPrjLgcGetManyExpenseRspItemMdl()
            {
                EmployeeProjectExpenseID = x.EmployeeProjectExpenseID,
                EmployeeProjectExpenseName = x.EmployeeProjectExpenseName,
                EmployeeProjectExpensePredictAmount = x.EmployeeProjectExpensePredictAmount,
                EmployeeProjectExpenseActualAmount = x.EmployeeProjectExpenseActualAmount,
                EmployeeProjectExpenseBillNumber = x.EmployeeProjectExpenseBillNumber,
                EmployeeProjectExpenseBillTime = x.EmployeeProjectExpenseBillTime,
                EmployeeProjectExpenseRemark = x.EmployeeProjectExpenseRemark,
            })
            .ToList();
        rspObj.EipProjectExpenseList = eipV1HttpPmsGetProjectExpensesRspObj.DataList
            .Select(item => new MbsWrkPrjLgcGetEipExpenseRspItemMdl()
            {
                ProjectExpenseApprovalStatus = item.ApprovalStatus,
                ProjectExpenseApplicant = item.Applicant,
                ProjectExpenseDate = item.ExpenseDate,
                ProjectExpenseReason = item.ExpenseReason,
                ProjectExpenseParticipants = item.Participants,
                ProjectExpenseAmount = item.ExpenseAmount,
            })
            .ToList();
        rspObj.EipProjectTravelExpenseList = eipV1HttpPmsGetProjectTravelExpenseRspObj.DataList
            .Select(item => new MbsWrkPrjLgcGetEipTravelExpenseRspItemMdl()
            {
                ProjectTravelExpenseApprovalStatus = item.ApprovalStatus,
                ProjectTravelExpenseApplicant = item.Applicant,
                ProjectTravelExpenseTravelDate = item.TravelDate,
                ProjectTravelExpenseTravelRoute = item.TravelRoute,
                ProjectTravelExpenseWorkDescription = item.WorkDescription,
                ProjectTravelExpenseTransportationTool = item.TransportationTool,
                ProjectTravelExpenseTransportationAmount = item.TransportationAmount,
                ProjectTravelExpenseMileage = item.Mileage,
                ProjectTravelExpenseAccommodationAmount = item.AccommodationAmount,
                ProjectTravelExpenseSpecialExpenseReason = item.SpecialExpenseReason,
                ProjectTravelExpenseSpecialExpenseAmount = item.SpecialExpenseAmount,
                ProjectTravelExpenseTotalAmount = item.TotalAmount,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-新增專案支出</summary>
    public async Task<MbsWrkPrjLgcAddExpenseRspMdl> AddExpenseAsync(MbsWrkPrjLgcAddExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcAddExpenseRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
      {
          DbEmployeeProjectMemberRoleEnum.Saler,
          DbEmployeeProjectMemberRoleEnum.ProjectManager,
          DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
      };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案支出-資料庫-新增
        var coEmpPrjExpDbAddReqObj = new CoEmpPrjExpDbAddReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectExpenseName = reqObj.EmployeeProjectExpenseName,
            EmployeeProjectExpensePredictAmount = reqObj.EmployeeProjectExpensePredictAmount,
            EmployeeProjectExpenseActualAmount = reqObj.EmployeeProjectExpenseActualAmount,
            EmployeeProjectExpenseBillNumber = reqObj.EmployeeProjectExpenseBillNumber,
            EmployeeProjectExpenseBillTime = reqObj.EmployeeProjectExpenseBillTime,
            EmployeeProjectExpenseRemark = reqObj.EmployeeProjectExpenseRemark,
        };
        var coEmpPrjExpDbAddRspObj = await this._coEmpProjectExpenseDb.AddAsync(coEmpPrjExpDbAddReqObj);
        if (coEmpPrjExpDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案支出</summary>
    public async Task<MbsWrkPrjLgcUpdateExpenseRspMdl> UpdateExpenseAsync(MbsWrkPrjLgcUpdateExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjLgcUpdateExpenseRspMdl
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

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案支出-資料庫-取得員工專案ID
        var coEmpPrjExpDbGetEmployeeProjectIdReqObj = new CoEmpPrjExpDbGetEmployeeProjectIdReqMdl()
        {
            EmployeeProjectExpenseID = reqObj.EmployeeProjectExpenseID,
        };
        var coEmpPrjExpDbGetEmployeeProjectIdRspObj = await this._coEmpProjectExpenseDb.GetEmployeeProjectIdAsync(coEmpPrjExpDbGetEmployeeProjectIdReqObj);
        if (coEmpPrjExpDbGetEmployeeProjectIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #region 第二層 權限判斷

        // 該接口允許的角色
        var allowMemberRoleList = new List<DbEmployeeProjectMemberRoleEnum>
        {
          DbEmployeeProjectMemberRoleEnum.Saler,
          DbEmployeeProjectMemberRoleEnum.ProjectManager,
          DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
        };

        // db, 核心-員工-專案成員-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl()
        {
            EmployeeProjectID = coEmpPrjExpDbGetEmployeeProjectIdRspObj.EmployeeProjectID,
            EmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否有權限
        var hasPermission = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Any(x => allowMemberRoleList.Contains(x.EmployeeProjectMemberRole));
        if (hasPermission == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-專案支出-資料庫-更新
        var coEmpPrjExpDbUpdateReqObj = new CoEmpPrjExpDbUpdateReqMdl()
        {
            EmployeeProjectExpenseID = reqObj.EmployeeProjectExpenseID,
            EmployeeProjectExpenseName = reqObj.EmployeeProjectExpenseName,
            EmployeeProjectExpensePredictAmount = reqObj.EmployeeProjectExpensePredictAmount,
            EmployeeProjectExpenseActualAmount = reqObj.EmployeeProjectExpenseActualAmount,
            EmployeeProjectExpenseBillNumber = reqObj.EmployeeProjectExpenseBillNumber,
            EmployeeProjectExpenseBillTime = reqObj.EmployeeProjectExpenseBillTime,
            EmployeeProjectExpenseRemark = reqObj.EmployeeProjectExpenseRemark,
        };
        var coEmpPrjExpDbUpdateRspObj = await this._coEmpProjectExpenseDb.UpdateAsync(coEmpPrjExpDbUpdateReqObj);
        if (coEmpPrjExpDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion
}
