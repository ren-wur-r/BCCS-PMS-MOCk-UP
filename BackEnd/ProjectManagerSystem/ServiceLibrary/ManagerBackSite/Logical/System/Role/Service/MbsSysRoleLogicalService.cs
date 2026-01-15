using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomPermissionKind;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Permission.Format;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Manager.DB.Department.Format;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.Core.Manager.DB.Region.Format;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.Core.Manager.DB.Role.Format;
using ServiceLibrary.Core.Manager.DB.Role.Service;
using ServiceLibrary.Core.Manager.DB.RolePermission.Format;
using ServiceLibrary.Core.Manager.DB.RolePermission.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Service;

/// <summary>管理者後台-系統設定-角色-邏輯服務</summary>
public class MbsSysRoleLogicalService : IMbsSysRoleLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsSysRoleLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-地區-資料庫服務</summary>
    private readonly ICoMgrRegionDbService _coMgrRegionDb;

    /// <summary>核心-管理者-部門-資料庫服務</summary>
    private readonly ICoMgrDepartmentDbService _coMgrDepartmentDb;

    /// <summary>核心-管理者-角色-資料庫服務</summary>
    private readonly ICoMgrRoleDbService _coMgrRoleDb;

    /// <summary>核心-管理者-角色權限-資料庫-服務</summary>
    private readonly ICoMgrRolePermissionDbService _coMgrRolePermissionDb;

    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-目錄權限-資料庫服務</summary>
    private readonly ICoEmpPermissionDbService _coEmpPermissionDb;

    #endregion

    #region ManagerBackSite

    /// <summary>管理者後台-通用-邏輯服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #endregion

    #region This

    /// <summary>原子-管理者後台-列舉</summary>
    private readonly DataModelLibrary.Database.AtomMenu.DbAtomMenuEnum _AtomMenu;

    #endregion

    /// <summary>建構式</summary>
    public MbsSysRoleLogicalService(
        ILogger<MbsSysRoleLogicalService> logger,
        // Core Manager
        ICoMgrRegionDbService coMgrRegionDb,
        ICoMgrDepartmentDbService coMgrDepartmentDb,
        ICoMgrRoleDbService coMgrRoleDb,
        ICoMgrRolePermissionDbService coMgrRolePermissionDb,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpPermissionDbService coEmpPermissionDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrRegionDb = coMgrRegionDb;
        this._coMgrDepartmentDb = coMgrDepartmentDb;
        this._coMgrRoleDb = coMgrRoleDb;
        this._coMgrRolePermissionDb = coMgrRolePermissionDb;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpPermissionDb = coEmpPermissionDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._AtomMenu = DataModelLibrary.Database.AtomMenu.DbAtomMenuEnum.SystemRole;
    }

    /// <summary>管理者後台-系統設定-角色-邏輯-取得多筆</summary>
    public async Task<MbsSysRolLgcGetManyRspMdl> GetManyAsync(MbsSysRolLgcGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysRolLgcGetManyRspMdl
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

        // db, 核心-管理者-角色-資料庫-取得[筆數]從[管理者後台-系統-角色]
        var coMgrRolDbGetCountFromMbsSysRolReqObj = new CoMgrRolDbGetCountFromMbsSysRolReqMdl()
        {
            ManagerRoleName = reqObj.ManagerRoleName,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
        };
        var coMgrRolDbGetCountFromMbsSysRolRspObj = await this._coMgrRoleDb.GetCountFromMbsSysRolAsync(coMgrRolDbGetCountFromMbsSysRolReqObj);
        if (coMgrRolDbGetCountFromMbsSysRolRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrRolDbGetCountFromMbsSysRolRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerRoleList = new List<MbsSysRolLgcGetManyItemRspMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-角色-資料庫-取得多筆從[管理者後台-系統-角色]
        var coMgrRolDbGetManyFromMbsSysRolReqObj = new CoMgrRolDbGetManyFromMbsSysRolReqMdl()
        {
            ManagerRoleName = reqObj.ManagerRoleName,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coMgrRolDbGetManyFromMbsSysRolRspObj = await this._coMgrRoleDb.GetManyFromMbsSysRolAsync(coMgrRolDbGetManyFromMbsSysRolReqObj);
        if (coMgrRolDbGetManyFromMbsSysRolRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得多筆[筆數]依[管理者角色ID]
        var coEmpInfDbGetManyCountByManagerRoleIdReqObj = new CoEmpInfDbGetManyCountByManagerRoleIdReqMdl()
        {
            ManagerRoleIdList = coMgrRolDbGetManyFromMbsSysRolRspObj.ManagerRoleList
                .Select(x => x.ManagerRoleID)
                .Distinct()
                .ToList()
        };
        var coEmpInfDbGetManyCountByManagerRoleIdRspObj = await this._coEmpInfoDb.GetManyCountByManagerRoleIdAsync(coEmpInfDbGetManyCountByManagerRoleIdReqObj);
        if (coEmpInfDbGetManyCountByManagerRoleIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRoleList =
            (
                from r in coMgrRolDbGetManyFromMbsSysRolRspObj.ManagerRoleList
                from e in coEmpInfDbGetManyCountByManagerRoleIdRspObj.ManagerRoleList
                    .Where(e => e.ManagerRoleID == r.ManagerRoleID)
                    .DefaultIfEmpty()
                select new MbsSysRolLgcGetManyItemRspMdl()
                {
                    ManagerRoleID = r.ManagerRoleID,
                    ManagerRoleName = r.ManagerRoleName,
                    ManagerRoleIsEnable = r.ManagerRoleIsEnable,
                    EmployeeCount = e == default ? 0 : e.Count,
                }
            ).ToList();
        rspObj.TotalCount = coMgrRolDbGetCountFromMbsSysRolRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-角色-邏輯-取得</summary>
    public async Task<MbsSysRolLgcGetRspMdl> GetAsync(MbsSysRolLgcGetReqMdl reqObj)
    {
        var rspObj = new MbsSysRolLgcGetRspMdl
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

        // db, 核心-管理者-角色-資料庫-取得
        var coMgrRolDbGetReqObj = new CoMgrRolDbGetReqMdl()
        {
            ManagerRoleID = reqObj.ManagerRoleID
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
            ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID,
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
            ManagerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID,
        };
        var coMgrDptDbGetRspObj = await this._coMgrDepartmentDb.GetAsync(coMgrDptDbGetReqObj);
        if (coMgrDptDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得[筆數]依[管理者角色ID]
        var coEmpInfDbGetCountByManagerRoleIdReqObj = new CoEmpInfDbGetCountByManagerRoleIdReqMdl()
        {
            ManagerRoleID = reqObj.ManagerRoleID
        };
        var coEmpInfDbGetCountByManagerRoleIdRspObj = await this._coEmpInfoDb.GetCountByManagerRoleIdAsync(coEmpInfDbGetCountByManagerRoleIdReqObj);
        if (coEmpInfDbGetCountByManagerRoleIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        var employeeCount = coEmpInfDbGetCountByManagerRoleIdRspObj.Count;

        // db, 核心-管理者-角色權限-資料庫-取得多筆
        var coMgrRpDbGetManyReqObj = new CoMgrRpDbGetManyReqMdl()
        {
            ManagerRoleID = reqObj.ManagerRoleID,
        };
        var coMgrRpDbGetManyRspObj = await this._coMgrRolePermissionDb.GetManyAsync(coMgrRpDbGetManyReqObj);
        if (coMgrRpDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }


        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRoleName = coMgrRolDbGetRspObj.ManagerRoleName;
        rspObj.ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID;
        rspObj.ManagerRegionName = coMgrRgnDbGetRspObj.ManagerRegionName;
        rspObj.ManagerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID;
        rspObj.ManagerDepartmentName = coMgrDptDbGetRspObj.ManagerDepartmentName;
        rspObj.ManagerRoleRemark = coMgrRolDbGetRspObj.ManagerRoleRemark;
        rspObj.ManagerRoleIsEnable = coMgrRolDbGetRspObj.ManagerRoleIsEnable;
        rspObj.EmployeeCount = employeeCount;
        rspObj.ManagerRolePermissionList = coMgrRpDbGetManyRspObj.ManagerRolePermissionList
            .Select(x => new MbsSysRolLgcGetRspItemMdl()
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

    /// <summary>管理者後台-系統設定-角色-邏輯-新增</summary>
    public async Task<MbsSysRolLgcAddRspMdl> AddAsync(MbsSysRolLgcAddReqMdl reqObj)
    {
        var rspObj = new MbsSysRolLgcAddRspMdl
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

        // db, 核心-管理者-角色-資料庫-是否存在
        var coMgrRolDbExistReqObj = new CoMgrRolDbExistReqMdl()
        {
            ManagerRoleName = reqObj.ManagerRoleName,
        };
        var coMgrRolDbExistRspObj = await this._coMgrRoleDb.ExistAsync(coMgrRolDbExistReqObj);
        if (coMgrRolDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrRolDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 名稱重複
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-角色-資料庫-新增
        var coMgrRolDbAddReqObj = new CoMgrRolDbAddReqMdl()
        {
            ManagerRoleName = reqObj.ManagerRoleName,
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleRemark = reqObj.ManagerRoleRemark,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
        };
        var coMgrRolDbAddRspObj = await this._coMgrRoleDb.AddAsync(coMgrRolDbAddReqObj);
        if (coMgrRolDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-角色權限-資料庫-新增多筆
        var coMgrRpDbAddManyReqObj = new CoMgrRpDbAddManyReqMdl()
        {
            ManagerRolePermissionList = reqObj.ManagerRolePermissionList
                .Select(x => new CoMgrRpDbAddManyReqItemMdl()
                {
                    ManagerRoleID = coMgrRolDbAddRspObj.ManagerRoleID,
                    AtomMenu = x.AtomMenu,
                    ManagerRegionID = x.ManagerRegionID,
                    AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                    AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                    AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                    AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
                })
                .ToList()
        };
        var coMgrRpDbAddManyRspObj = await this._coMgrRolePermissionDb.AddManyAsync(coMgrRpDbAddManyReqObj);
        if (coMgrRpDbAddManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // recover
            // db, 核心-管理者-角色-資料庫-移除
            var coMgrRolDbRemoveReqObj = new CoMgrRolDbRemoveReqMdl()
            {
                ManagerRoleID = coMgrRolDbAddRspObj.ManagerRoleID
            };
            var coMgrRolDbRemoveRspObj = await this._coMgrRoleDb.RemoveAsync(coMgrRolDbRemoveReqObj);
            if (coMgrRolDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            return rspObj;
        }
        if (coMgrRpDbAddManyRspObj.IsSuccess == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // recover
            // db, 核心-管理者-角色-資料庫-移除
            var coMgrRolDbRemoveReqObj = new CoMgrRolDbRemoveReqMdl()
            {
                ManagerRoleID = coMgrRolDbAddRspObj.ManagerRoleID
            };
            var coMgrRolDbRemoveRspObj = await this._coMgrRoleDb.RemoveAsync(coMgrRolDbRemoveReqObj);
            if (coMgrRolDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRoleID = coMgrRolDbAddRspObj.ManagerRoleID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-角色-邏輯-更新</summary>
    public async Task<MbsSysRolLgcUpdateRspMdl> UpdateAsync(MbsSysRolLgcUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysRolLgcUpdateRspMdl
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

        // 判斷名稱是否需要更新
        if (!string.IsNullOrWhiteSpace(reqObj.ManagerRoleName))
        {
            // 名稱需要更新，檢查名稱是否重複

            // 核心-管理者-角色-資料庫-是否存在
            var coMgrRolDbExistReqObj = new CoMgrRolDbExistReqMdl()
            {
                ManagerRoleID = reqObj.ManagerRoleID,
                ManagerRoleName = reqObj.ManagerRoleName,
            };
            var coMgrRolDbExistRspObj = await this._coMgrRoleDb.ExistAsync(coMgrRolDbExistReqObj);
            if (coMgrRolDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrRolDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 名稱重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-角色-資料庫-更新
        var coMgrRolDbUpdateReqObj = new CoMgrRolDbUpdateReqMdl()
        {
            ManagerRoleID = reqObj.ManagerRoleID,
            ManagerRoleName = reqObj.ManagerRoleName,
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleRemark = reqObj.ManagerRoleRemark,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
        };
        var coMgrRolDbUpdateRspObj = await this._coMgrRoleDb.UpdateAsync(coMgrRolDbUpdateReqObj);
        if (coMgrRolDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷權限是否需要更新
        if (reqObj.ManagerRolePermissionList != null)
        {
            // db, 核心-管理者-角色權限-資料庫-移除多筆
            var coMgrRpDbRemoveManyReqObj = new CoMgrRpDbRemoveManyReqMdl()
            {
                ManagerRoleID = reqObj.ManagerRoleID
            };
            var coMgrRpDbRemoveManyRspObj = await this._coMgrRolePermissionDb.RemoveManyAsync(coMgrRpDbRemoveManyReqObj);
            if (coMgrRpDbRemoveManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-管理者-角色權限-資料庫-新增多筆
            var coMgrRpDbAddManyReqObj = new CoMgrRpDbAddManyReqMdl()
            {
                ManagerRolePermissionList = reqObj.ManagerRolePermissionList
                    .Select(x => new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = reqObj.ManagerRoleID,
                        AtomMenu = x.AtomMenu,
                        ManagerRegionID = x.ManagerRegionID,
                        AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                        AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                        AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                        AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
                    })
                    .ToList()
            };
            var coMgrRpDbAddManyRspObj = await this._coMgrRolePermissionDb.AddManyAsync(coMgrRpDbAddManyReqObj);
            if (coMgrRpDbAddManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-資訊-資料庫-取得多筆[員工ID]依[管理者角色ID]
            var coEmpInfDbGetManyIdByManagerRoleIdReqObj = new CoEmpInfDbGetManyIdByManagerRoleIdReqMdl()
            {
                ManagerRoleID = reqObj.ManagerRoleID
            };
            var coEmpInfDbGetManyIdByManagerRoleIdRspObj = await this._coEmpInfoDb.GetManyIdByManagerRoleIdAsync(coEmpInfDbGetManyIdByManagerRoleIdReqObj);
            if (coEmpInfDbGetManyIdByManagerRoleIdRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 如果有員工使用此角色，則更新這些員工的目錄權限
            if (coEmpInfDbGetManyIdByManagerRoleIdRspObj.EmployeeIdList != null &&
                coEmpInfDbGetManyIdByManagerRoleIdRspObj.EmployeeIdList.Any())
            {
                foreach (var employeeID in coEmpInfDbGetManyIdByManagerRoleIdRspObj.EmployeeIdList)
                {
                    // 刪除該員工的所有目錄權限
                    var coEmpPmnDbRemoveManyReqObj = new CoEmpPmnDbRemoveManyReqMdl()
                    {
                        EmployeeID = employeeID
                    };
                    var coEmpPmnDbRemoveManyRspObj = await this._coEmpPermissionDb.RemoveManyAsync(coEmpPmnDbRemoveManyReqObj);
                    if (coEmpPmnDbRemoveManyRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // 為該員工新增目錄權限（從角色權限複製）
                    var coEmpPmnDbAddManyReqObj = new CoEmpPmnDbAddManyReqMdl()
                    {
                        EmployeePermissionList = reqObj.ManagerRolePermissionList
                            .Select(x => new CoEmpPmnDbAddManyReqItemMdl()
                            {
                                EmployeeID = employeeID,
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
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }
}
