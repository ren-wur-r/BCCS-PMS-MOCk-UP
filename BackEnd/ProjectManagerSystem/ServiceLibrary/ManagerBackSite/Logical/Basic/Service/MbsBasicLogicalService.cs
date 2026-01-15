using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Permission.Format;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Service;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Format;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Service;
using ServiceLibrary.Core.Employee.Logical.Format;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;
using ServiceLibrary.Core.Employee.DB.Project.Format;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;
using ServiceLibrary.Core.Manager.DB.Company.Format;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Service;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Service;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;
using ServiceLibrary.Core.Manager.DB.Contacter.Format;
using ServiceLibrary.Core.Manager.DB.Contacter.Service;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Service;
using ServiceLibrary.Core.Manager.DB.Department.Format;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.Core.Manager.DB.Product.Format;
using ServiceLibrary.Core.Manager.DB.Product.Service;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;
using ServiceLibrary.Core.Manager.DB.Region.Format;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.Core.Manager.DB.Role.Format;
using ServiceLibrary.Core.Manager.DB.Role.Service;
using ServiceLibrary.Core.Manager.DB.RolePermission.Format;
using ServiceLibrary.Core.Manager.DB.RolePermission.Service;
using ServiceLibrary.ManagerBackSite.Logical.Basic.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Service;

/// <summary>管理者後台-基本-邏輯服務</summary>
public class MbsBasicLogicalService : IMbsBasicLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsBasicLogicalService> _logger;

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-目錄權限-資料庫服務</summary>
    private readonly ICoEmpPermissionDbService _coEmpPmnDb;

    /// <summary>核心-員工-商機原始資料-資料庫服務</summary>
    private readonly ICoEmpPipelineOriginalDbService _coEmpPipelineOriginalDb;

    /// <summary>核心-員工-專案成員-資料庫服務</summary>
    private readonly ICoEmpProjectMemberDbService _coEmpProjectMemberDb;

    /// <summary>核心-員工-登入資訊-記憶體服務</summary>
    private readonly ICoEmpLoginInfoMemoryService _coEmpLoginInfoMemory;

    /// <summary>核心-員工-邏輯服務</summary>
    private readonly ICoEmployeeLogicalService _coEmployeeLogical;

    /// <summary>核心-員工-專案-資料庫服務</summary>
    private readonly ICoEmpProjectDbService _coEmpProjectDb;

    /// <summary>核心-員工-專案里程碑-資料庫服務</summary>
    private readonly ICoEmpProjectStoneDbService _coEmpProjectStoneDb;

    /// <summary>核心-員工-專案里程碑工項-資料庫服務</summary>
    private readonly ICoEmpProjectStoneJobDbService _coEmpProjectStoneJobDb;

    #endregion

    #region Core Manager

    /// <summary>核心-管理者-公司-資料庫服務</summary>
    private readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    /// <summary>核心-管理者-關係公司-資料庫服務</summary>
    private readonly ICoMgrCompanyAffiliateDbService _coMgrCompanyAffiliateDb;

    /// <summary>核心-管理者-公司營業地點-資料庫服務</summary>
    private readonly ICoMgrCompanyLocationDbService _coMgrCompanyLocationDb;

    /// <summary>核心-管理者-公司主分類-資料庫服務</summary>
    private readonly ICoMgrCompanyMainKindDbService _coMgrCompanyMainKindDb;

    /// <summary>核心-管理者-公司子分類-資料庫服務</summary>
    private readonly ICoMgrCompanySubKindDbService _coMgrCompanySubKindDb;

    /// <summary>核心-管理者-窗口-資料庫服務</summary>
    private readonly ICoMgrContacterDbService _coMgrCttDb;

    /// <summary>核心-管理者-窗口開發評等原因-資料庫服務</summary>
    private readonly ICoMgrContacterRatingReasonDbService _coMgrContacterRatingReasonDb;

    /// <summary>核心-管理者-部門-資料庫服務</summary>
    private readonly ICoMgrDepartmentDbService _coMgrDptDb;

    /// <summary>核心-管理者-產品-資料庫服務</summary>
    private readonly ICoMgrProductDbService _coMgrPrdDb;

    /// <summary>核心-管理者-產品主分類-資料庫服務</summary>
    private readonly ICoMgrProductMainKindDbService _coMgrPmkDb;

    /// <summary>核心-管理者-產品子分類-資料庫服務</summary>
    private readonly ICoMgrProductSubKindDbService _coMgrPskDb;

    /// <summary>核心-管理者-產品規格-資料庫服務</summary>
    private readonly ICoMgrProductSpecificationDbService _coMgrPsDb;

    /// <summary>核心-管理者-區域-資料庫服務</summary>
    private readonly ICoMgrRegionDbService _coMgrRgnDb;

    /// <summary>核心-管理者-角色-資料庫服務</summary>
    private readonly ICoMgrRoleDbService _coMgrRolDb;

    /// <summary>核心-管理者-角色權限-資料庫服務</summary>
    private readonly ICoMgrRolePermissionDbService _coMgrRpDb;

    #endregion

    #region ManagerBackSite

    /// <summary>管理者後台-通用-邏輯服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #endregion

    /// <summary>建構式</summary>
    public MbsBasicLogicalService(
        ILogger<MbsBasicLogicalService> logger,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpPermissionDbService coEmpPmnDb,
        ICoEmpPipelineOriginalDbService coEmpPipelineOriginalDb,
        ICoEmpProjectMemberDbService coEmpProjectMemberDb,
        ICoEmpLoginInfoMemoryService coEmpLoginInfoMemory,
        ICoEmployeeLogicalService coEmployeeLogical,
        ICoEmpProjectDbService coEmpProjectDb,
        ICoEmpProjectStoneDbService coEmpProjectStoneDb,
        ICoEmpProjectStoneJobDbService coEmpProjectStoneJobDb,
        // Core Manager
        ICoMgrCompanyDbService coMgrCompanyDb,
        ICoMgrCompanyAffiliateDbService coMgrCompanyAffiliateDb,
        ICoMgrCompanyLocationDbService coMgrCompanyLocationDb,
        ICoMgrCompanyMainKindDbService coMgrCompanyMainKindDb,
        ICoMgrCompanySubKindDbService coMgrCompanySubKindDb,
        ICoMgrContacterDbService coMgrCttDb,
        ICoMgrContacterRatingReasonDbService coMgrContacterRatingReasonDb,
        ICoMgrDepartmentDbService coMgrDptDb,
        ICoMgrProductDbService coMgrPrdDb,
        ICoMgrProductMainKindDbService coMgrPmkDb,
        ICoMgrProductSubKindDbService coMgrPskDb,
        ICoMgrProductSpecificationDbService coMgrPsDb,
        ICoMgrRegionDbService coMgrRgnDb,
        ICoMgrRoleDbService coMgrRolDb,
        ICoMgrRolePermissionDbService coMgrRpDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Employee
        this._coEmpPmnDb = coEmpPmnDb;
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpPipelineOriginalDb = coEmpPipelineOriginalDb;
        this._coEmpProjectMemberDb = coEmpProjectMemberDb;
        this._coEmpLoginInfoMemory = coEmpLoginInfoMemory;
        this._coEmployeeLogical = coEmployeeLogical;
        this._coEmpProjectDb = coEmpProjectDb;
        this._coEmpProjectStoneDb = coEmpProjectStoneDb;
        this._coEmpProjectStoneJobDb = coEmpProjectStoneJobDb;
        // Core Manager
        this._coMgrCompanyDb = coMgrCompanyDb;
        this._coMgrCompanyAffiliateDb = coMgrCompanyAffiliateDb;
        this._coMgrCompanyLocationDb = coMgrCompanyLocationDb;
        this._coMgrCompanyMainKindDb = coMgrCompanyMainKindDb;
        this._coMgrCompanySubKindDb = coMgrCompanySubKindDb;
        this._coMgrCttDb = coMgrCttDb;
        this._coMgrContacterRatingReasonDb = coMgrContacterRatingReasonDb;
        this._coMgrDptDb = coMgrDptDb;
        this._coMgrPrdDb = coMgrPrdDb;
        this._coMgrPmkDb = coMgrPmkDb;
        this._coMgrPskDb = coMgrPskDb;
        this._coMgrPsDb = coMgrPsDb;
        this._coMgrRgnDb = coMgrRgnDb;
        this._coMgrRolDb = coMgrRolDb;
        this._coMgrRpDb = coMgrRpDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
    }

    /// <summary>管理者後台-基本-邏輯-登入</summary>
    public async Task<MbsBscLgcLoginRspMdl> LoginAsync(MbsBscLgcLoginReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcLoginRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // logical, 核心-員工-邏輯-取得密文密碼
        var coEmpLgcGetCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl()
        {
            EmployeePlainPassword = reqObj.EmployeePassword,
        };
        var coEmpLgcGetCypherPasswordRspObj = this._coEmployeeLogical.GetCypherPassword(coEmpLgcGetCypherPasswordReqObj);
        if (coEmpLgcGetCypherPasswordRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得[ID]
        var coEmpInfDbGetIdReqObj = new CoEmpInfDbGetIdReqMdl()
        {
            EmployeeAccount = reqObj.EmployeeAccount,
            EmployeePassword = coEmpLgcGetCypherPasswordRspObj.EmployeeCypherPassword,
        };
        var coEmpInfDbGetIdRspObj = await this._coEmpInfoDb.GetIdAsync(coEmpInfDbGetIdReqObj);
        if (coEmpInfDbGetIdRspObj == default)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.AccountOrPasswordError;

            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // mbs, 管理者後台-通用-邏輯-登出
        var mbsCmnLgcLogoutReqObj = new MbsCmnLgcLogoutReqMdl()
        {
            EmployeeID = coEmpInfDbGetIdRspObj.EmployeeID,
        };
        var mbsCmnLgcLogoutRspObj = await this._mbsCommonLogical.LogoutAsync(mbsCmnLgcLogoutReqObj);
        if (mbsCmnLgcLogoutRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-取得
        var coEmpInfoDbGetReqObj = new CoEmpInfDbGetReqMdl()
        {
            EmployeeID = coEmpInfDbGetIdRspObj.EmployeeID
        };
        var coEmpInfoDbGetRspObj = await this._coEmpInfoDb.GetAsync(coEmpInfoDbGetReqObj);
        if (coEmpInfoDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否啟用
        if (coEmpInfoDbGetRspObj.EmployeeIsEnable == false)
        {
            //帳號未啟用
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.AccountDisabled;
            return rspObj;
        }

        // logical, 核心-員工-取得登入令牌
        var coEmpLgclGetLoginTokenReqObj = new CoEmpLgcGetLoginTokenReqMdl()
        {
            EmployeeID = coEmpInfDbGetIdRspObj.EmployeeID,
        };
        var coEmpLgclGetLoginTokenRspObj = this._coEmployeeLogical.GetLoginToken(coEmpLgclGetLoginTokenReqObj);
        if (coEmpLgclGetLoginTokenRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // memory, 核心-員工-新增
        var coEmpLiMemAddReqObj = new CoEmpLiMemAddReqMdl()
        {
            EmployeeLoginToken = coEmpLgclGetLoginTokenRspObj.LoginToken,
            EmployeeID = coEmpInfDbGetIdRspObj.EmployeeID,
        };
        var coEmpLiMemAddRspObj = this._coEmpLoginInfoMemory.Add(coEmpLiMemAddReqObj);
        if (coEmpLiMemAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpLiMemAddRspObj.IsSuccess == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeLoginToken = coEmpLgclGetLoginTokenRspObj.LoginToken;
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-登出</summary>
    public async Task<MbsBscLgcLogoutRspMdl> LogoutAsync(MbsBscLgcLogoutReqMdl reqObj)
    {
        var rspObj = new MbsBscLgcLogoutRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // mbs, 管理者後台-通用-邏輯-登出
        var mbsCmnLgcLogoutReqObj = new MbsCmnLgcLogoutReqMdl()
        {
            EmployeeID = coEmpLiMemGetRspObj.EmployeeID,
        };
        var mbsCmnLgcLogoutRspObj = await this._mbsCommonLogical.LogoutAsync(mbsCmnLgcLogoutReqObj);
        if (mbsCmnLgcLogoutRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return await Task.FromResult(rspObj);
    }

    /// <summary>管理者後台-基本-邏輯-心跳</summary>
    public async Task<MbsBscLgcHeartbeatRspMdl> HeartbeatAsync(MbsBscLgcHeartbeatReqMdl reqObj)
    {
        var rspObj = new MbsBscLgcHeartbeatRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-更新到期時間
        var coEmpLiMemUpdateExpiredTimeReqObj = new CoEmpLiMemUpdateExpiredTimeReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
        };
        var coEmpLiMemUpdateExpiredTimeRspObj = this._coEmpLoginInfoMemory.UpdateExpiredTime(coEmpLiMemUpdateExpiredTimeReqObj);
        if (coEmpLiMemUpdateExpiredTimeRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpLiMemUpdateExpiredTimeRspObj.IsSuccess == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return await Task.FromResult(rspObj);
    }

    #region Employee

    /// <summary>管理者後台-基本-邏輯-取得員工資訊</summary>
    public async Task<MbsBscLgcGetEmployeeRspMdl> GetEmployeeAsync(MbsBscLgcGetEmployeeReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetEmployeeRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得
        var coEmpInfoDbGetReqObj = new CoEmpInfDbGetReqMdl()
        {
            EmployeeID = coEmpLiMemGetRspObj.EmployeeID,
        };
        var coEmpInfoDbGetRspObj = await this._coEmpInfoDb.GetAsync(coEmpInfoDbGetReqObj);
        if (coEmpInfoDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-角色-資料庫-取得
        var coMgrRolDbGetReqObj = new CoMgrRolDbGetReqMdl()
        {
            ManagerRoleID = coEmpInfoDbGetRspObj.ManagerRoleID,
        };
        var coMgrRolDbGetRspObj = await this._coMgrRolDb.GetAsync(coMgrRolDbGetReqObj);
        if (coMgrRolDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-地區-資料庫-取得[名稱]
        var coMgrRgnDbGetNameReqObj = new CoMgrRgnDbGetNameReqMdl()
        {
            ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID,
        };
        var coMgrRgnDbGetNameRspObj = await this._coMgrRgnDb.GetNameAsync(coMgrRgnDbGetNameReqObj);
        if (coMgrRgnDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-部門-資料庫-取得[名稱]
        var coMgrDptDbGetNameReqObj = new CoMgrDptDbGetNameReqMdl()
        {
            ManagerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID,
        };
        var coMgrDptDbGetNameRspObj = await this._coMgrDptDb.GetNameAsync(coMgrDptDbGetNameReqObj);
        if (coMgrDptDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-目錄權限-資料庫-取得多筆
        var coEmpPmnDbGetManyReqObj = new CoEmpPmnDbGetManyReqMdl()
        {
            EmployeeID = coEmpLiMemGetRspObj.EmployeeID,
        };
        var coEmpPmnDbGetManyRspObj = await this._coEmpPmnDb.GetManyAsync(coEmpPmnDbGetManyReqObj);
        if (coEmpPmnDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeAccount = coEmpInfoDbGetRspObj.EmployeeAccount;
        rspObj.EmployeeName = coEmpInfoDbGetRspObj.EmployeeName;
        rspObj.ManagerRoleID = coEmpInfoDbGetRspObj.ManagerRoleID;
        rspObj.ManagerRoleName = coMgrRolDbGetRspObj.ManagerRoleName;
        rspObj.ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID;
        rspObj.ManagerRegionName = coMgrRgnDbGetNameRspObj.ManagerRegionName;
        rspObj.ManagerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID;
        rspObj.ManagerDepartmentName = coMgrDptDbGetNameRspObj.ManagerDepartmentName;
        rspObj.ManagerBackSiteMenuPermissionList = coEmpPmnDbGetManyRspObj.EmployeePermissionList
            .Select(x => new MbsBscLgcGetEmployeeRspItemMdl()
            {
                AtomMenu = x.AtomMenu,
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-修改員工密碼</summary>
    public async Task<MbsBscLgcUpdatePasswordRspMdl> UpdateEmployeePasswordAsync(MbsBscLgcUpdatePasswordReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcUpdatePasswordRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得密碼
        var coEmpInfDbGetPasswordReqObj = new CoEmpInfDbGetPasswordReqMdl()
        {
            EmployeeID = coEmpLiMemGetRspObj.EmployeeID,
        };
        var coEmpInfDbGetPasswordRspObj = await this._coEmpInfoDb.GetPasswordAsync(coEmpInfDbGetPasswordReqObj);
        if (coEmpInfDbGetPasswordRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // logical, 核心-員工-邏輯-取得密文密碼(舊密碼)
        var coEmpLgcGetCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl()
        {
            EmployeePlainPassword = reqObj.OldPassword,
        };
        var coEmpLgcGetCypherPasswordRspObj = this._coEmployeeLogical.GetCypherPassword(coEmpLgcGetCypherPasswordReqObj);
        if (coEmpLgcGetCypherPasswordRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 比較密碼
        if (coEmpInfDbGetPasswordRspObj.EmployeePassword != coEmpLgcGetCypherPasswordRspObj.EmployeeCypherPassword)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.AccountOrPasswordError;
            return rspObj;
        }

        // logical, 核心-員工-邏輯-取得密文密碼(新密碼)
        var coEmpLgcGetNewCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl()
        {
            EmployeePlainPassword = reqObj.NewPassword,
        };
        var coEmpLgcGetNewCypherPasswordRspObj = this._coEmployeeLogical.GetCypherPassword(coEmpLgcGetNewCypherPasswordReqObj);
        if (coEmpLgcGetNewCypherPasswordRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-更新
        var coEmpInfDbUpdateReqObj = new CoEmpInfDbUpdateReqMdl()
        {
            EmployeeID = coEmpLiMemGetRspObj.EmployeeID,
            EmployeePassword = coEmpLgcGetNewCypherPasswordRspObj.EmployeeCypherPassword
        };
        var coEmpInfDbUpdateRspObj = await this._coEmpInfoDb.UpdateAsync(coEmpInfDbUpdateReqObj);
        if (coEmpInfDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得多筆員工資訊</summary>
    public async Task<MbsBscLgcGetManyEmployeeInfoRspMdl> GetManyEmployeeInfoAsync(MbsBscLgcGetManyEmployeeInfoReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyEmployeeInfoRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得多筆從[管理者後台-基本]
        var coEmpInfDbGetManyFromMbsBscReqObj = new CoEmpInfDbGetManyFromMbsBscReqMdl()
        {
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeName = reqObj.EmployeeName,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE
        };
        var coEmpInfDbGetManyFromMbsBscRspObj = await this._coEmpInfoDb.GetManyFromMbsBscAsync(coEmpInfDbGetManyFromMbsBscReqObj);
        if (coEmpInfDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeList = coEmpInfDbGetManyFromMbsBscRspObj.EmployeeList
            .Select(x => new MbsBasicLgcGetManyEmployeeInfoRspItemMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
                EmployeeID = x.EmployeeID,
                EmployeeName = x.EmployeeName,
                EmployeeIsEnable = x.EmployeeIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProjectMember

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員</summary>
    public async Task<MbsBscLgcGetManyEmployeeProjectMemberRspMdl> GetManyEmployeeProjectMemberAsync(MbsBscLgcGetManyEmployeeProjectMemberReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetManyEmployeeProjectMemberRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

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

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfoDbGetManyReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
                .Select(x => x.EmployeeID)
                .Distinct()
                .ToList(),
        };
        var coEmpInfoDbGetManyRspObj = await this._coEmpInfoDb.GetManyNameAsync(coEmpInfoDbGetManyReqObj);
        if (coEmpInfoDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectMemberList =
            (
                from pm in coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
                from e in coEmpInfoDbGetManyRspObj.EmployeeList
                        .Where(ex => ex.EmployeeID == pm.EmployeeID)
                select new MbsBscLgcGetManyEmployeeProjectMemberRspItemMdl()
                {
                    EmployeeProjectMemberRole = pm.EmployeeProjectMemberRole,
                    EmployeeID = pm.EmployeeID,
                    EmployeeName = e.EmployeeName,
                }
            ).ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProject

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案</summary>
    public async Task<MbsBscLgcGetManyProjectRspMdl> GetManyEmployeeProjectAsync(MbsBscLgcGetManyProjectReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyProjectRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-專案-資料庫-取得多筆從[管理者後台-基本]
        var coEmpPrjDbGetManyFromMbsBscReqObj = new CoEmpPrjDbGetManyFromMbsBscReqMdl()
        {
            EmployeeProjectName = reqObj.EmployeeProjectName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coEmpPrjDbGetManyFromMbsBscRspObj = await this._coEmpProjectDb.GetManyFromMbsBscAsync(coEmpPrjDbGetManyFromMbsBscReqObj);
        if (coEmpPrjDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectList = coEmpPrjDbGetManyFromMbsBscRspObj.EmployeeProjectList
            .Select(x => new MbsBscLgcGetManyProjectRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                EmployeeProjectName = x.EmployeeProjectName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProjectStone

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案里程碑</summary>
    public async Task<MbsBscLgcGetManyProjectStoneRspMdl> GetManyEmployeeProjectStoneAsync(MbsBscLgcGetManyProjectStoneReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyProjectStoneRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-專案里程碑-資料庫-取得多筆從[管理者後台-基本]
        var coEmpPsDbGetManyFromMbsBscReqObj = new CoEmpPsDbGetManyFromMbsBscReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneName = reqObj.EmployeeProjectStoneName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE
        };
        var coEmpPsDbGetManyFromMbsBscRspObj = await this._coEmpProjectStoneDb.GetManyFromMbsBscAsync(coEmpPsDbGetManyFromMbsBscReqObj);
        if (coEmpPsDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneList = coEmpPsDbGetManyFromMbsBscRspObj.EmployeeProjectStoneList
            .Select(x => new MbsBscLgcGetManyProjectStoneRspItemMdl()
            {
                EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                EmployeeProjectStoneName = x.EmployeeProjectStoneName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProjectStoneJob

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案里程碑工項</summary>
    public async Task<MbsBscLgcGetManyProjectStoneJobRspMdl> GetManyEmployeeProjectStoneJobAsync(MbsBscLgcGetManyProjectStoneJobReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyProjectStoneJobRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案里程碑工項-資料庫-取得多筆從[管理者後台-基本]
        var coEmpPsjDbGetManyFromMbsBscReqObj = new CoEmpPsjDbGetManyFromMbsBscReqMdl()
        {
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobName = reqObj.EmployeeProjectStoneJobName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coEmpPsjDbGetManyFromMbsBscRspObj = await this._coEmpProjectStoneJobDb.GetManyFromMbsBscAsync(coEmpPsjDbGetManyFromMbsBscReqObj);
        if (coEmpPsjDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneJobList = coEmpPsjDbGetManyFromMbsBscRspObj.EmployeeProjectStoneJobList
            .Select(x => new MbsBscLgcGetManyProjectStoneJobRspItemMdl()
            {
                EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerDepartment

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者部門</summary>
    public async Task<MbsBscLgcGetManyDepartmentRspMdl> GetManyManagerDepartmentAsync(MbsBscLgcGetManyDepartmentReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyDepartmentRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-部門-資料庫-取得多筆從[管理者後台-基本]
        var coMgrDptDbGetManyFromMbsBscReqObj = new CoMgrDptDbGetManyFromMbsBscReqMdl()
        {
            ManagerDepartmentName = reqObj.ManagerDepartmentName,
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrDptDbGetManyRspObj = await this._coMgrDptDb.GetManyFromMbsBscDptAsync(coMgrDptDbGetManyFromMbsBscReqObj);
        if (coMgrDptDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerDepartmentList = coMgrDptDbGetManyRspObj.ManagerDepartmentList
            .Select(x => new MbsBscLgcGetManyDepartmentRspItemMdl()
            {
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerDepartmentIsEnable = x.ManagerDepartmentIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得全部管理者部門</summary>
    public async Task<MbsBscLgcGetAllDepartmentRspMdl> GetAllManagerDepartmentAsync(MbsBscLgcGetAllDepartmentReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetAllDepartmentRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // db, 核心-管理者-部門-資料庫-取得全部從[管理者後台-基本]
        var coMgrDptDbGetAllFromMbsBscReqObj = new CoMgrDptDbGetAllFromMbsBscReqMdl()
        {
        };
        var coMgrDptDbGetAllFromMbsBscRspObj = await this._coMgrDptDb.GetAllFromMbsBscDptAsync(coMgrDptDbGetAllFromMbsBscReqObj);
        if (coMgrDptDbGetAllFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerDepartmentList = coMgrDptDbGetAllFromMbsBscRspObj.ManagerDepartmentList
            .Select(x => new MbsBscLgcGetAllDepartmentRspItemMdl()
            {
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerRegion

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者地區</summary>
    public async Task<MbsBscLgcGetManyRegionRspMdl> GetManyManagerRegionAsync(MbsBscLgcGetManyRegionReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyRegionRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-區域-資料庫-取得多筆從[管理者後台-基本]
        var coMgrRgnDbGetManyFromMbsBscReqObj = new CoMgrRgnDbGetManyFromMbsBscReqMdl()
        {
            ManagerRegionName = reqObj.ManagerRegionName,
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrRgnDbGetManyFromMbsBscRspObj = await this._coMgrRgnDb.GetManyFromMbsBscRgnAsync(coMgrRgnDbGetManyFromMbsBscReqObj);
        if (coMgrRgnDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRegionList = coMgrRgnDbGetManyFromMbsBscRspObj.ManagerRegionList
            .Select(x => new MbsBscLgcGetManyRegionRspItemMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerRegionIsEnable = x.ManagerRegionIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得全部管理者地區</summary>
    public async Task<MbsBscLgcGetAllRegionRspMdl> GetAllManagerRegionAsync(MbsBscLgcGetAllRegionReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetAllRegionRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // db, 核心-管理者-地區-資料庫-取得全部從[管理者後台-基本]
        var coMgrRgnDbGetAllFromMbsBscReqObj = new CoMgrRgnDbGetAllFromMbsBscReqMdl()
        {
        };
        var coMgrRgnDbGetAllFromMbsBscRspObj = await this._coMgrRgnDb.GetAllFromMbsBscRgnAsync(coMgrRgnDbGetAllFromMbsBscReqObj);
        if (coMgrRgnDbGetAllFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRegionList = coMgrRgnDbGetAllFromMbsBscRspObj.ManagerRegionList
            .Select(x => new MbsBscLgcGetAllRegionRspItemMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerRole

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者角色</summary>
    public async Task<MbsBscLgcGetManyRoleRspMdl> GetManyManagerRoleAsync(MbsBscLgcGetManyRoleReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyRoleRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-角色-資料庫-取得多筆從[管理者後台-基本]
        var coMgrRolDbGetManyFromMbsBscReqObj = new CoMgrRolDbGetManyFromMbsBscReqMdl()
        {
            ManagerRoleName = reqObj.ManagerRoleName,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrRolDbGetManyFromMbsBscRspObj = await this._coMgrRolDb.GetManyFromMbsBscRolAsync(coMgrRolDbGetManyFromMbsBscReqObj);
        if (coMgrRolDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRoleList = coMgrRolDbGetManyFromMbsBscRspObj.ManagerRoleList
            .Select(x => new MbsBscLgcGetManyRoleRspItemMdl()
            {
                ManagerRoleID = x.ManagerRoleID,
                ManagerRoleName = x.ManagerRoleName,
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerRoleIsEnable = x.ManagerRoleIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerRolePermission

    /// <summary>管理者後台-基本-邏輯-取得多筆角色權限從[管理者後台-基本-角色ID]</summary>
    public async Task<MbsBscLgcGetManyRolePermissionFromRoleIdRspMdl> GetManyRolePermissionFromRoleAsync(MbsBscLgcGetManyRolePermissionFromRoleIdReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetManyRolePermissionFromRoleIdRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-角色權限-資料庫-取得多筆從[管理者後台-基本-角色]
        var coMgrRpDbGetManyFromMbsBscReqObj = new CoMgrRpDbGetManyFromMbsBscReqMdl()
        {
            ManagerRoleID = reqObj.ManagerRoleID,
        };
        var coMgrRpDbGetManyFromMbsBscRspObj = await this._coMgrRpDb.GetManyFromMbsBscRpAsync(coMgrRpDbGetManyFromMbsBscReqObj);
        if (coMgrRpDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRolePermissionList = coMgrRpDbGetManyFromMbsBscRspObj.ManagerRolePermissionList
            .Select(x => new MbsBscLgcGetManyRolePermissionFromRoleIdRspItemMdl()
            {
                AtomMenu = x.AtomMenu,
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerProduct

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者產品主分類</summary>
    public async Task<MbsBscLgcGetManyProductMainKindRspMdl> GetManyManagerProductMainKindAsync(MbsBscLgcGetManyProductMainKindReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyProductMainKindRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-基本]
        var coMgrPmkDbGetManyFromMbsBscReqObj = new CoMgrPmkDbGetManyFromMbsBscReqMdl()
        {
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrPmkDbGetManyFromMbsBscRspObj = await this._coMgrPmkDb.GetManyFromMbsBscAsync(coMgrPmkDbGetManyFromMbsBscReqObj);
        if (coMgrPmkDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindList = coMgrPmkDbGetManyFromMbsBscRspObj.ManagerProductMainKindList
            .Select(x => new MbsBscLgcGetManyProductMainKindRspItemMdl()
            {
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductMainKindIsEnable = x.ManagerProductMainKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者產品子分類</summary>
    public async Task<MbsBscLgcGetManyProductSubKindRspMdl> GetManyManagerProductSubKindAsync(MbsBscLgcGetManyProductSubKindReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyProductSubKindRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-基本]
        var coMgrPskDbGetManyFromMbsBscReqObj = new CoMgrPskDbGetManyFromMbsBscReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrPskDbGetManyFromMbsBscRspObj = await this._coMgrPskDb.GetManyFromMbsBscAsync(coMgrPskDbGetManyFromMbsBscReqObj);
        if (coMgrPskDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductSubKindList = coMgrPskDbGetManyFromMbsBscRspObj.ManagerProductSubKindList
            .Select(x => new MbsBscLgcGetManyProductSubKindRspItemMdl()
            {
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductSubKindIsEnable = x.ManagerProductSubKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得多筆產品</summary>
    public async Task<MbsBscLgcGetManyProductRspMdl> GetManyProductAsync(MbsBscLgcGetManyProductReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyProductRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-產品-資料庫-取得多筆從[管理者後台-基本]
        var coMgrPrdDbGetManyFromMbsBscReqObj = new CoMgrPrdDbGetManyFromMbsBscReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductName = reqObj.ManagerProductName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrPrdDbGetManyFromMbsBscRspObj = await this._coMgrPrdDb.GetManyFromMbsBscAsync(coMgrPrdDbGetManyFromMbsBscReqObj);
        if (coMgrPrdDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得多筆
        var coMgrPmkDbGetManyReqObj = new CoMgrPmkDbGetManyReqMdl()
        {
            ManagerProductMainKindIdList = coMgrPrdDbGetManyFromMbsBscRspObj.ManagerProductList
                .Select(x => x.ManagerProductMainKindID)
                .Distinct()
                .ToList()
        };
        var coMgrPmkDbGetManyRspObj = await this._coMgrPmkDb.GetManyAsync(coMgrPmkDbGetManyReqObj);
        if (coMgrPmkDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-取得多筆[名稱]
        var coMgrPskDbGetManyNameReqObj = new CoMgrPskDbGetManyNameReqMdl()
        {
            ManagerProductSubKindIDList = coMgrPrdDbGetManyFromMbsBscRspObj.ManagerProductList
                .Select(x => x.ManagerProductSubKindID)
                .Distinct()
                .ToList()
        };
        var coMgrPskDbGetManyNameRspObj = await this._coMgrPskDb.GetManyNameAsync(coMgrPskDbGetManyNameReqObj);
        if (coMgrPskDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductList = (
            from product in coMgrPrdDbGetManyFromMbsBscRspObj.ManagerProductList
            from mainKind in coMgrPmkDbGetManyRspObj.ManagerProductMainKindList
                .Where(m => m.ManagerProductMainKindID == product.ManagerProductMainKindID)
            from subKind in coMgrPskDbGetManyNameRspObj.ManagerProductSubKindList
                .Where(s => s.ManagerProductSubKindID == product.ManagerProductSubKindID)
            select new MbsBscLgcGetManyProductRspItemMdl()
            {
                ManagerProductID = product.ManagerProductID,
                ManagerProductName = product.ManagerProductName,
                ManagerProductIsEnable = product.ManagerProductIsEnable,
                ManagerProductMainKindID = product.ManagerProductMainKindID,
                ManagerProductMainKindName = mainKind.ManagerProductMainKindName,
                ManagerProductMainKindCommissionRate = mainKind.ManagerProductMainKindCommissionRate,
                ManagerProductSubKindID = product.ManagerProductSubKindID,
                ManagerProductSubKindName = subKind.ManagerProductSubKindName
            }
        ).ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得多筆產品規格</summary>
    public async Task<MbsBscLgcGetManyProductSpecificationRspMdl> GetManyProductSpecificationAsync(MbsBscLgcGetManyProductSpecificationReqMdl reqObj)
    {

        const int PAGE_SIZE = 5;

        var rspObj = new MbsBscLgcGetManyProductSpecificationRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-基本]
        var coMgrPsDbGetManyFromMbsBscReqObj = new CoMgrPsDbGetManyFromMbsBscReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrPsDbGetManyFromMbsBscRspObj = await this._coMgrPsDb.GetManyFromMbsBscAsync(coMgrPsDbGetManyFromMbsBscReqObj);
        if (coMgrPsDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductSpecificationList = coMgrPsDbGetManyFromMbsBscRspObj.ManagerProductSpecificationList
            .Select(x => new MbsBscLgcGetManyProductSpecificationRspItemMdl()
            {
                ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerContacterRatingReason

    /// <summary>管理者後台-基本-邏輯-取得多筆窗口開發評等原因</summary>
    public async Task<MbsBscLgcGetManyContacterRatingReasonRspMdl> GetManyManagerContacterRatingReasonAsync(MbsBscLgcGetManyContacterRatingReasonReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyContacterRatingReasonRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-基本]
        var coMgrCttRtgRsnDbGetManyFromMbsBscReqObj = new CoMgrCttRtgRsnDbGetManyFromMbsBscReqMdl()
        {
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName,
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrCttRtgRsnDbGetManyFromMbsBscRspObj = await this._coMgrContacterRatingReasonDb.GetManyFromMbsBscAsync(coMgrCttRtgRsnDbGetManyFromMbsBscReqObj);
        if (coMgrCttRtgRsnDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingReasonList = coMgrCttRtgRsnDbGetManyFromMbsBscRspObj.ManagerContacterRatingReasonList?
            .Select(x => new MbsBscLgcGetManyContacterRatingReasonRspItemMdl()
            {
                ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                ManagerContacterRatingReasonName = x.ManagerContacterRatingReasonName,
                ManagerContacterRatingReasonStatus = x.ManagerContacterRatingReasonStatus,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerCompanyMainKind

    /// <summary>管理者後台-基本-邏輯-取得多筆公司主分類</summary>
    public async Task<MbsBscLgcGetManyCompanyMainKindRspMdl> GetManyManagerCompanyMainKindAsync(MbsBscLgcGetManyCompanyMainKindReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyCompanyMainKindRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-基本]
        var coMgrCmpMainKdDbGetManyFromMbsBscReqObj = new CoMgrCmpMainKdDbGetManyFromMbsBscReqMdl()
        {
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrCmpMainKdDbGetManyFromMbsBscRspObj = await this._coMgrCompanyMainKindDb.GetManyFromMbsBscAsync(coMgrCmpMainKdDbGetManyFromMbsBscReqObj);
        if (coMgrCmpMainKdDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyMainKindList = coMgrCmpMainKdDbGetManyFromMbsBscRspObj.ManagerCompanyMainKindList?
            .Select(x => new MbsBscLgcGetManyCompanyMainKindRspItemMdl()
            {
                ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                ManagerCompanyMainKindName = x.ManagerCompanyMainKindName,
                ManagerCompanyMainKindIsEnable = x.ManagerCompanyMainKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerCompanySubKind

    /// <summary>管理者後台-基本-邏輯-取得多筆公司子分類</summary>
    public async Task<MbsBscLgcGetManyCompanySubKindRspMdl> GetManyManagerCompanySubKindAsync(MbsBscLgcGetManyCompanySubKindReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyCompanySubKindRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-基本]
        var coMgrCmpSubKdDbGetManyFromMbsBscReqObj = new CoMgrCmpSubKdDbGetManyFromMbsBscReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName,
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrCmpSubKdDbGetManyFromMbsBscRspObj = await this._coMgrCompanySubKindDb.GetManyFromMbsBscAsync(coMgrCmpSubKdDbGetManyFromMbsBscReqObj);
        if (coMgrCmpSubKdDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanySubKindList = coMgrCmpSubKdDbGetManyFromMbsBscRspObj.ManagerCompanySubKindList?
            .Select(x => new MbsBscLgcGetManyCompanySubKindRspItemMdl()
            {
                ManagerCompanySubKindID = x.ManagerCompanySubKindID,
                ManagerCompanySubKindName = x.ManagerCompanySubKindName,
                ManagerCompanySubKindIsEnable = x.ManagerCompanySubKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerContacter

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者窗口</summary>
    public async Task<MbsBscLgcGetManyManagerContacterRspMdl> GetManyManagerContacterAsync(MbsBscLgcGetManyManagerContacterReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyManagerContacterRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫-取得多筆從[管理者後台-基本]
        var coMgrCttDbGetManyFromMbsBscReqObj = new CoMgrCttDbGetManyFromMbsBscReqMdl()
        {
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrCttDbGetManyFromMbsBscRspObj = await this._coMgrCttDb.GetManyFromMbsBscAsync(coMgrCttDbGetManyFromMbsBscReqObj);
        if (coMgrCttDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterList = coMgrCttDbGetManyFromMbsBscRspObj.ManagerContacterList?
            .Select(x => new MbsBscLgcGetManyContacterRspItemMdl()
            {
                ManagerContacterID = x.ManagerContacterID,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得管理者窗口</summary>
    public async Task<MbsBscLgcGetManagerContacterRspMdl> GetManagerContacterAsync(MbsBscLgcGetManagerContacterReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetManagerContacterRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫-取得
        var coMgrCttDbGetReqObj = new CoMgrCttDbGetReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID,
        };
        var coMgrCttDbGetRspObj = await this._coMgrCttDb.GetAsync(coMgrCttDbGetReqObj);
        if (coMgrCttDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterID = coMgrCttDbGetRspObj.ManagerContacterID;
        rspObj.ManagerCompanyID = coMgrCttDbGetRspObj.ManagerCompanyID;
        rspObj.ManagerContacterName = coMgrCttDbGetRspObj.ManagerContacterName;
        rspObj.ManagerContacterEmail = coMgrCttDbGetRspObj.ManagerContacterEmail;
        rspObj.ManagerContacterPhone = coMgrCttDbGetRspObj.ManagerContacterPhone;
        rspObj.ManagerContacterDepartment = coMgrCttDbGetRspObj.ManagerContacterDepartment;
        rspObj.ManagerContacterJobTitle = coMgrCttDbGetRspObj.ManagerContacterJobTitle;
        rspObj.ManagerContacterTelephone = coMgrCttDbGetRspObj.ManagerContacterTelephone;
        rspObj.ManagerContacterStatus = coMgrCttDbGetRspObj.ManagerContacterStatus;
        rspObj.ManagerContacterIsConsent = coMgrCttDbGetRspObj.ManagerContacterIsConsent;
        rspObj.AtomRatingKind = coMgrCttDbGetRspObj.AtomRatingKind;
        rspObj.ManagerContacterCreateEmployeeID = coMgrCttDbGetRspObj.ManagerContacterCreateEmployeeID;
        rspObj.ManagerContacterRemark = coMgrCttDbGetRspObj.ManagerContacterRemark;
        return rspObj;
    }

    #endregion

    #region ManagerCompany

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者公司</summary>
    public async Task<MbsBscLgcGetManyCompanyRspMdl> GetManyManagerCompanyAsync(MbsBscLgcGetManyCompanyReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyCompanyRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得多筆從[管理者後台-基本]
        var coMgrCmpDbGetManyFromMbsBscReqObj = new CoMgrCmpDbGetManyFromMbsBscReqMdl()
        {
            ManagerCompanyName = reqObj.ManagerCompanyName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coMgrCmpDbGetManyFromMbsBscRspObj = await this._coMgrCompanyDb.GetManyFromMbsBscAsync(coMgrCmpDbGetManyFromMbsBscReqObj);
        if (coMgrCmpDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyList = coMgrCmpDbGetManyFromMbsBscRspObj.ManagerCompanyList
            .Select(x => new MbsBscLgcGetManyCompanyRspItemMdl()
            {
                ManagerCompanyID = x.ManagerCompanyID,
                ManagerCompanyName = x.ManagerCompanyName,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得多筆公司名稱從[商機原始]</summary>
    public async Task<MbsBscLgcGetManyCompanyNameFromPipelineOriginalRspMdl> GetManyManagerCompanyNameFromPipelineOriginalAsync(MbsBscLgcGetManyCompanyNameFromPipelineOriginalReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyCompanyNameFromPipelineOriginalRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-員工-商機原始資料-資料庫-取得多筆公司名稱從[管理者後台-基本]
        var coEmpPplOgnDbGetManyMgrComNameFromMbsBscReqObj = new CoEmpPplOgnDbGetManyMgrComNameFromMbsBscReqMdl()
        {
            ManagerCompanyName = reqObj.ManagerCompanyName,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE,
        };
        var coEmpPplOgnDbGetManyMgrComNameFromMbsBscRspObj = await this._coEmpPipelineOriginalDb.GetManyManagerCompanyNameFromMbsBscAsync(coEmpPplOgnDbGetManyMgrComNameFromMbsBscReqObj);
        if (coEmpPplOgnDbGetManyMgrComNameFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyNameList = coEmpPplOgnDbGetManyMgrComNameFromMbsBscRspObj.ManagerCompanyNameList;
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得管理者公司</summary>
    public async Task<MbsBscLgcGetCompanyRspMdl> GetManagerCompanyAsync(MbsBscLgcGetCompanyReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetCompanyRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得
        var coMgrCmpDbGetReqObj = new CoMgrCmpDbGetReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };
        var coMgrCmpDbGetRspObj = await this._coMgrCompanyDb.GetAsync(coMgrCmpDbGetReqObj);
        if (coMgrCmpDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得[名稱]
        var coMgrCmpMainKdDbGetNameReqObj = new CoMgrCmpMainKdDbGetNameReqMdl()
        {
            ManagerCompanyMainKindID = coMgrCmpDbGetRspObj.ManagerCompanyMainKindID
        };
        var coMgrCmpMainKdDbGetNameRspObj = await this._coMgrCompanyMainKindDb.GetNameAsync(coMgrCmpMainKdDbGetNameReqObj);
        if (coMgrCmpMainKdDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-取得[名稱]
        var coMgrCmpSubKdDbGetNameReqObj = new CoMgrCmpSubKdDbGetNameReqMdl()
        {
            ManagerCompanySubKindID = coMgrCmpDbGetRspObj.ManagerCompanySubKindID
        };
        var coMgrCmpSubKdDbGetNameRspObj = await this._coMgrCompanySubKindDb.GetNameAsync(coMgrCmpSubKdDbGetNameReqObj);
        if (coMgrCmpSubKdDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyID = coMgrCmpDbGetRspObj.ManagerCompanyID;
        rspObj.ManagerCompanyUnifiedNumber = coMgrCmpDbGetRspObj.ManagerCompanyUnifiedNumber;
        rspObj.ManagerCompanyName = coMgrCmpDbGetRspObj.ManagerCompanyName;
        rspObj.AtomManagerCompanyStatus = coMgrCmpDbGetRspObj.AtomManagerCompanyStatus;
        rspObj.ManagerCompanyMainKindID = coMgrCmpDbGetRspObj.ManagerCompanyMainKindID;
        rspObj.ManagerCompanyMainKindName = coMgrCmpMainKdDbGetNameRspObj.ManagerCompanyMainKindName;
        rspObj.ManagerCompanySubKindID = coMgrCmpDbGetRspObj.ManagerCompanySubKindID;
        rspObj.ManagerCompanySubKindName = coMgrCmpSubKdDbGetNameRspObj.ManagerCompanySubKindName;
        rspObj.AtomCustomerGrade = coMgrCmpDbGetRspObj.AtomCustomerGrade;
        rspObj.AtomSecurityGrade = coMgrCmpDbGetRspObj.AtomSecurityGrade;
        rspObj.AtomEmployeeRange = coMgrCmpDbGetRspObj.AtomEmployeeRange;
        return rspObj;
    }

    #endregion

    #region ManagerCompanyLocation

    /// <summary>管理者後台-基本-邏輯-取得多筆公司營業地點</summary>
    public async Task<MbsBscLgcGetManyCompanyLocationRspMdl> GetManyManagerCompanyLocationAsync(MbsBscLgcGetManyCompanyLocationReqMdl reqObj)
    {
        //查詢筆數限制
        const int PAGE_SIZE = 5;

        // 預設回應
        var rspObj = new MbsBscLgcGetManyCompanyLocationRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-公司營業地點-資料庫-取得多筆從[管理者後台-基本]
        var coMgrCmpLocDbGetManyFromMbsBscReqObj = new CoMgrCmpLocDbGetManyFromMbsBscReqMdl()
        {
            ManagerCompanyLocationName = reqObj.ManagerCompanyLocationName,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            PageIndex = reqObj.PageIndex,
            PageSize = PAGE_SIZE
        };
        var coMgrCmpLocDbGetManyFromMbsBscRspObj = await this._coMgrCompanyLocationDb.GetManyFromMbsBscAsync(coMgrCmpLocDbGetManyFromMbsBscReqObj);
        if (coMgrCmpLocDbGetManyFromMbsBscRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyLocationList = coMgrCmpLocDbGetManyFromMbsBscRspObj.ManagerCompanyLocationList
            .Select(x => new MbsBscLgcGetManyCompanyLocationRspItemMdl()
            {
                ManagerCompanyLocationID = x.ManagerCompanyLocationID,
                ManagerCompanyLocationName = x.ManagerCompanyLocationName,
                ManagerCompanyLocationIsDeleted = x.ManagerCompanyLocationIsDeleted
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-基本-邏輯-取得公司營業地點</summary>
    public async Task<MbsBscLgcGetCompanyLocationRspMdl> GetManagerCompanyLocationAsync(MbsBscLgcGetCompanyLocationReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscLgcGetCompanyLocationRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // memory, 核心-員工-登入資訊-記憶體-取得
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var coEmpLiMemGetRspObj = this._coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (coEmpLiMemGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        // db, 核心-管理者-公司營業地點-資料庫-取得
        var coMgrCmpLocDbGetReqObj = new CoMgrCmpLocDbGetReqMdl()
        {
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            ManagerCompanyLocationIsDeleted = reqObj.ManagerCompanyLocationIsDeleted,
        };
        var coMgrCmpLocDbGetRspObj = await this._coMgrCompanyLocationDb.GetAsync(coMgrCmpLocDbGetReqObj);
        if (coMgrCmpLocDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyLocationID = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationID;
        rspObj.ManagerCompanyID = coMgrCmpLocDbGetRspObj.ManagerCompanyID;
        rspObj.ManagerCompanyLocationName = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationName;
        rspObj.AtomCity = coMgrCmpLocDbGetRspObj.AtomCity;
        rspObj.ManagerCompanyLocationAddress = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationAddress;
        rspObj.ManagerCompanyLocationTelephone = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationTelephone;
        rspObj.ManagerCompanyLocationStatus = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationStatus;
        return rspObj;
    }

    #endregion
}
