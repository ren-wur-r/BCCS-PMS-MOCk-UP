using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Permission.Format;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Employee.Logical.Format;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Manager.DB.Department.Format;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.Core.Manager.DB.Region.Format;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.Core.Manager.DB.Role.Format;
using ServiceLibrary.Core.Manager.DB.Role.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Service;

/// <summary>管理者後台-系統-員工-邏輯服務</summary>
public class MbsSysEmployeeLogicalService : IMbsSysEmployeeLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSysEmployeeLogicalService> _logger;

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-目錄權限-資料庫服務</summary>
    private readonly ICoEmpPermissionDbService _coEmpPermissionDb;

    /// <summary>核心-員工-邏輯服務</summary>
    private readonly ICoEmployeeLogicalService _coEmpLogical;

    #endregion

    #region Core Manager

    /// <summary>核心-管理者-角色-資料庫服務</summary>
    private readonly ICoMgrRoleDbService _coMgrRoleDb;

    /// <summary>核心-管理者-地區-資料庫服務</summary>
    private readonly ICoMgrRegionDbService _coMgrRegionDb;

    /// <summary>核心-管理者-部門-資料庫服務</summary>
    private readonly ICoMgrDepartmentDbService _coMgrDepartmentDb;

    #endregion

    #region ManagerBackSite

    /// <summary>管理者後台-通用-邏輯服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #endregion

    #region This

    /// <summary>原子-管理者後台-列舉</summary>
    private readonly DbAtomMenuEnum _AtomMenu;

    #endregion

    /// <summary>建構式</summary>
    public MbsSysEmployeeLogicalService(
        ILogger<MbsSysEmployeeLogicalService> logger,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpPermissionDbService coEmpPermissionDb,
        ICoEmployeeLogicalService coEmpLogical,
        // Core Manager
        ICoMgrRoleDbService coMgrRoleDb,
        ICoMgrRegionDbService coMgrRegionDb,
        ICoMgrDepartmentDbService coMgrDepartmentDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpPermissionDb = coEmpPermissionDb;
        this._coEmpLogical = coEmpLogical;
        // Core Manager
        this._coMgrRoleDb = coMgrRoleDb;
        this._coMgrRegionDb = coMgrRegionDb;
        this._coMgrDepartmentDb = coMgrDepartmentDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._AtomMenu = DbAtomMenuEnum.SystemEmployee;
    }

    /// <summary>管理者後台-系統-員工-邏輯-取得多筆</summary>
    public async Task<MbsSysEmpLgcGetManyRspMdl> GetManyAsync(MbsSysEmpLgcGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpLgcGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._AtomMenu,
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

        // db, 核心-員工-資訊-資料庫-取得[筆數]從[管理者後台-系統-員工]
        var coEmpInfDbGetCountFromMbsSysEmpReqObj = new CoEmpInfDbGetCountFromMbsSysEmpReqMdl()
        {
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeAccount = reqObj.EmployeeAccount,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
        };
        var coEmpInfDbGetCountFromMbsSysEmpRspObj = await this._coEmpInfoDb.GetCountFromMbsSysEmpAsync(coEmpInfDbGetCountFromMbsSysEmpReqObj);
        if (coEmpInfDbGetCountFromMbsSysEmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coEmpInfDbGetCountFromMbsSysEmpRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeeList = new List<MbsSysEmpLgcGetManyItemRspMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得多筆從[管理者後台-系統-員工]
        var coEmpInfDbGetManyFromMbsSysEmpReqObj = new CoEmpInfDbGetManyFromMbsSysEmpReqMdl()
        {
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeAccount = reqObj.EmployeeAccount,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coEmpInfDbGetManyFromMbsSysEmpRspObj = await this._coEmpInfoDb.GetManyFromMbsSysEmpAsync(coEmpInfDbGetManyFromMbsSysEmpReqObj);
        if (coEmpInfDbGetManyFromMbsSysEmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeList = coEmpInfDbGetManyFromMbsSysEmpRspObj.EmployeeList
            .Select(e => new MbsSysEmpLgcGetManyItemRspMdl()
            {
                EmployeeID = e.EmployeeID,
                ManagerDepartmentID = e.ManagerDepartmentID,
                ManagerDepartmentName = e.ManagerDepartmentName,
                ManagerRoleID = e.ManagerRoleID,
                ManagerRoleName = e.ManagerRoleName,
                EmployeeName = e.EmployeeName,
                EmployeeAccount = e.EmployeeAccount,
                EmployeeIsEnable = e.EmployeeIsEnable,
            })
            .ToList();

        rspObj.TotalCount = coEmpInfDbGetCountFromMbsSysEmpRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統-員工-邏輯-取得</summary>
    public async Task<MbsSysEmpLgcGetRspMdl> GetAsync(MbsSysEmpLgcGetReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpLgcGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._AtomMenu,
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

        // db, 核心-員工-資訊-資料庫-取得
        var coEmpInfDbGetReqObj = new CoEmpInfDbGetReqMdl()
        {
            EmployeeID = reqObj.EmployeeID
        };
        var coEmpInfDbGetRspObj = await this._coEmpInfoDb.GetAsync(coEmpInfDbGetReqObj);
        if (coEmpInfDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-角色-資料庫-取得
        var coMgrRolDbGetReqObj = new CoMgrRolDbGetReqMdl()
        {
            ManagerRoleID = coEmpInfDbGetRspObj.ManagerRoleID
        };
        var coMgrRolDbGetRspObj = await this._coMgrRoleDb.GetAsync(coMgrRolDbGetReqObj);
        if (coMgrRolDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-地區-資料庫-取得
        var coMgrRgnDbGetReqObj = new CoMgrRgnDbGetReqMdl()
        {
            ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID
        };
        var coMgrRgnDbGetRspObj = await this._coMgrRegionDb.GetAsync(coMgrRgnDbGetReqObj);
        if (coMgrRgnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-部門-資料庫-取得
        var coMgrDptDbGetReqObj = new CoMgrDptDbGetReqMdl()
        {
            ManagerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID
        };
        var coMgrDptDbGetRspObj = await this._coMgrDepartmentDb.GetAsync(coMgrDptDbGetReqObj);
        if (coMgrDptDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        //db, 核心-員工-目錄權限-資料庫-取得多筆
        var coEmpPmnDbGetManyReqObj = new CoEmpPmnDbGetManyReqMdl()
        {
            EmployeeID = reqObj.EmployeeID,
        };
        var coEmpPmnDbGetManyRspObj = await this._coEmpPermissionDb.GetManyAsync(coEmpPmnDbGetManyReqObj);
        if (coEmpPmnDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeAccount = coEmpInfDbGetRspObj.EmployeeAccount;
        rspObj.EmployeeEmail = coEmpInfDbGetRspObj.EmployeeEmail;
        rspObj.EmployeeName = coEmpInfDbGetRspObj.EmployeeName;
        rspObj.ManagerRoleID = coEmpInfDbGetRspObj.ManagerRoleID;
        rspObj.ManagerRoleName = coMgrRolDbGetRspObj.ManagerRoleName;
        rspObj.ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID;
        rspObj.ManagerRegionName = coMgrRgnDbGetRspObj.ManagerRegionName;
        rspObj.ManagerDepartmentName = coMgrDptDbGetRspObj.ManagerDepartmentName;
        rspObj.EmployeeIsEnable = coEmpInfDbGetRspObj.EmployeeIsEnable;
        rspObj.EmployeePermissionList = coEmpPmnDbGetManyRspObj.EmployeePermissionList
            .Select(x => new MbsSysEmpLgcGetRspItemMdl()
            {
                AtomMenu = x.AtomMenu,
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-系統-員工-邏輯-新增</summary>
    public async Task<MbsSysEmpLgcAddRspMdl> AddAsync(MbsSysEmpLgcAddReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpLgcAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._AtomMenu,
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

        // logical, 核心-員工-邏輯-取得密文密碼
        var coEmpLgcGetCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl()
        {
            EmployeePlainPassword = reqObj.EmployeePassword,
        };
        var coEmpLgcGetCypherPasswordRspObj = this._coEmpLogical.GetCypherPassword(coEmpLgcGetCypherPasswordReqObj);
        if (coEmpLgcGetCypherPasswordRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-是否存在
        var coEmpInfDbExistReqObj = new CoEmpInfDbExistReqMdl()
        {
            EmployeeAccount = reqObj.EmployeeAccount,
        };
        var coEmpInfDbExistRspObj = await this._coEmpInfoDb.ExistAsync(coEmpInfDbExistReqObj);
        if (coEmpInfDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpInfDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 帳號重複
            rspObj.ErrorCode = MbsErrorCodeEnum.AccountDuplicate;
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-新增
        var coEmpInfDbAddReqObj = new CoEmpInfDbAddReqMdl()
        {
            EmployeeAccount = reqObj.EmployeeAccount,
            EmployeePassword = coEmpLgcGetCypherPasswordRspObj.EmployeeCypherPassword,
            EmployeeEmail = $"{reqObj.EmployeeAccount}@bccs.com.tw",
            EmployeeName = reqObj.EmployeeName,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
        };
        var coEmpInfDbAddRspObj = await this._coEmpInfoDb.AddAsync(coEmpInfDbAddReqObj);
        if (coEmpInfDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-目錄權限-資料庫-新增多筆
        var coEmpPmnDbAddManyReqObj = new CoEmpPmnDbAddManyReqMdl()
        {
            EmployeePermissionList = reqObj.EmployeePermissionList
                .Select(x => new CoEmpPmnDbAddManyReqItemMdl()
                {
                    EmployeeID = coEmpInfDbAddRspObj.EmployeeID,
                    AtomMenu = x.AtomMenu,
                    ManagerRegionID = x.ManagerRegionID,
                    AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                    AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                    AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                    AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
                })
                .ToList()
        };
        var coEmpPmnDbAddManyRspObj = await this._coEmpPermissionDb.AddManyAsync(coEmpPmnDbAddManyReqObj);
        if (coEmpPmnDbAddManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // recover
            // db, 核心-員工-資訊-資料庫-移除
            var coEmpInfDbRemoveReqObj = new CoEmpInfDbRemoveReqMdl()
            {
                EmployeeID = coEmpInfDbAddRspObj.EmployeeID,
            };
            var coEmpInfDbRemoveRspObj = await this._coEmpInfoDb.RemoveAsync(coEmpInfDbRemoveReqObj);
            if (coEmpInfDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            return rspObj;
        }
        if (coEmpPmnDbAddManyRspObj.IsSuccess == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // recover
            // db, 核心-員工-資訊-資料庫-移除
            var coEmpInfDbRemoveReqObj = new CoEmpInfDbRemoveReqMdl()
            {
                EmployeeID = coEmpInfDbAddRspObj.EmployeeID
            };
            var coEmpInfDbRemoveRspObj = await this._coEmpInfoDb.RemoveAsync(coEmpInfDbRemoveReqObj);
            if (coEmpInfDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeID = coEmpInfDbAddRspObj.EmployeeID;
        return rspObj;
    }

    /// <summary>管理者後台-系統-員工-邏輯-更新</summary>
    public async Task<MbsSysEmpLgcUpdateRspMdl> UpdateAsync(MbsSysEmpLgcUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpLgcUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._AtomMenu,
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

        // 判斷員工-密碼是否有值
        var cypherPassword = reqObj.EmployeePassword;
        if (!string.IsNullOrWhiteSpace(reqObj.EmployeePassword))
        {
            // logical, 核心-員工-邏輯-取得密文密碼
            var coEmpLgcGetCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl()
            {
                EmployeePlainPassword = reqObj.EmployeePassword,
            };
            var coEmpLgcGetCypherPasswordRspObj = this._coEmpLogical.GetCypherPassword(coEmpLgcGetCypherPasswordReqObj);
            if (coEmpLgcGetCypherPasswordRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            cypherPassword = coEmpLgcGetCypherPasswordRspObj.EmployeeCypherPassword;
        }

        // db, 核心-員工-資訊-資料庫-更新
        var coEmpInfDbUpdateReqObj = new CoEmpInfDbUpdateReqMdl()
        {
            EmployeeID = reqObj.EmployeeID,
            EmployeePassword = cypherPassword,
            EmployeeName = reqObj.EmployeeName,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
        };
        var coEmpInfDbUpdateRspObj = await this._coEmpInfoDb.UpdateAsync(coEmpInfDbUpdateReqObj);
        if (coEmpInfDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷權限是否需要更新
        if (reqObj.EmployeePermissionList != null)
        {
            // db, 核心-員工-目錄權限-資料庫-移除多筆
            var coEmpPmnDbRemoveManyReqObj = new CoEmpPmnDbRemoveManyReqMdl()
            {
                EmployeeID = reqObj.EmployeeID
            };
            var coEmpPmnDbRemoveManyRspObj = await this._coEmpPermissionDb.RemoveManyAsync(coEmpPmnDbRemoveManyReqObj);
            if (coEmpPmnDbRemoveManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-目錄權限-資料庫-新增多筆
            var coEmpPmnDbAddManyReqObj = new CoEmpPmnDbAddManyReqMdl()
            {
                EmployeePermissionList = reqObj.EmployeePermissionList
                .Select(x => new CoEmpPmnDbAddManyReqItemMdl()
                {
                    EmployeeID = reqObj.EmployeeID,
                    AtomMenu = x.AtomMenu,
                    ManagerRegionID = x.ManagerRegionID,
                    AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                    AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                    AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                    AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
                })
                .ToList()
            };
            var coEmpPmnDbAddManyRspObj = await this._coEmpPermissionDb.AddManyAsync(coEmpPmnDbAddManyReqObj);
            if (coEmpPmnDbAddManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }
}