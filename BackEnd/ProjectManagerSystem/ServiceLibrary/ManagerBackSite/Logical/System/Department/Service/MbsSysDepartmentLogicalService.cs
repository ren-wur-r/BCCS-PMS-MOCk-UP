using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomPermissionKind;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Department.Format;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Department.Service;

/// <summary>管理者後台-系統設定-部門-邏輯服務</summary>
public class MbsSysDepartmentLogicalService : IMbsSysDepartmentLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsSysDepartmentLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-部門-資料庫服務</summary>
    private readonly ICoMgrDepartmentDbService _coMgrDepartmentDb;

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
    public MbsSysDepartmentLogicalService(
        ILogger<MbsSysDepartmentLogicalService> logger,
        // Core Manager
        ICoMgrDepartmentDbService coMgrDepartmentDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrDepartmentDb = coMgrDepartmentDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._AtomMenu = DataModelLibrary.Database.AtomMenu.DbAtomMenuEnum.SystemDepartment;
    }

    /// <summary>管理者後台-系統設定-部門-邏輯-取得多筆</summary>
    public async Task<MbsSysDptLgcGetManyRspMdl> GetManyAsync(MbsSysDptLgcGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysDptLgcGetManyRspMdl
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

        // db, 核心-管理者-部門-資料庫-取得[筆數]從[管理者後台-系統-部門]
        var coMgrDptDbGetCountFromMbsSysDptReqObj = new CoMgrDptDbGetCountFromMbsSysDptReqMdl()
        {
            ManagerDepartmentName = reqObj.ManagerDepartmentName,
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
        };
        var coMgrDptDbGetCountFromMbsSysDptRspObj = await this._coMgrDepartmentDb.GetCountFromMbsSysDptAsync(coMgrDptDbGetCountFromMbsSysDptReqObj);
        if (coMgrDptDbGetCountFromMbsSysDptRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrDptDbGetCountFromMbsSysDptRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerDepartmentList = new List<MbsSysDptLgcGetManyItemRspMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-部門-資料庫-取得多筆從[管理者後台-系統-部門]
        var coMgrDptDbGetManyFromMbsSysDptReqObj = new CoMgrDptDbGetManyFromMbsSysDptReqMdl()
        {
            ManagerDepartmentName = reqObj.ManagerDepartmentName,
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coMgrDptDbGetManyFromMbsSysDptRspObj = await this._coMgrDepartmentDb.GetManyFromMbsSysDptAsync(coMgrDptDbGetManyFromMbsSysDptReqObj);
        if (coMgrDptDbGetManyFromMbsSysDptRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerDepartmentList = coMgrDptDbGetManyFromMbsSysDptRspObj.ManagerDepartmentList
            .Select(x => new MbsSysDptLgcGetManyItemRspMdl()
            {
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerDepartmentIsEnable = x.ManagerDepartmentIsEnable,
            })
            .ToList();
        rspObj.TotalCount = coMgrDptDbGetCountFromMbsSysDptRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-部門-邏輯-取得</summary>
    public async Task<MbsSysDptLgcGetRspMdl> GetAsync(MbsSysDptLgcGetReqMdl reqObj)
    {
        var rspObj = new MbsSysDptLgcGetRspMdl
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

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-部門-資料庫-取得
        var coMgrDptDbGetReqObj = new CoMgrDptDbGetReqMdl()
        {
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
        };
        var coMgrDptDbGetRspObj = await this._coMgrDepartmentDb.GetAsync(coMgrDptDbGetReqObj);
        if (coMgrDptDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerDepartmentName = coMgrDptDbGetRspObj.ManagerDepartmentName;
        rspObj.ManagerDepartmentIsEnable = coMgrDptDbGetRspObj.ManagerDepartmentIsEnable;

        return rspObj;
    }

    /// <summary>管理者後台-系統設定-部門-邏輯-新增</summary>
    public async Task<MbsSysDptLgcAddRspMdl> AddAsync(MbsSysDptLgcAddReqMdl reqObj)
    {
        var rspObj = new MbsSysDptLgcAddRspMdl
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

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 核心-管理者-部門-資料庫-是否存在
        var coMgrDptDbExistReqObj = new CoMgrDptDbExistReqMdl()
        {
            ManagerDepartmentName = reqObj.ManagerDepartmentName,
        };
        var coMgrDptDbExistRspObj = await this._coMgrDepartmentDb.ExistAsync(coMgrDptDbExistReqObj);
        if (coMgrDptDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrDptDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 名稱重複
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-部門-資料庫-新增
        var coMgrDptDbAddReqObj = new CoMgrDptDbAddReqMdl()
        {
            ManagerDepartmentName = reqObj.ManagerDepartmentName,
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
        };
        var coMgrDptDbAddRspObj = await this._coMgrDepartmentDb.AddAsync(coMgrDptDbAddReqObj);
        if (coMgrDptDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-部門-邏輯-更新</summary>
    public async Task<MbsSysDptLgcUpdateRspMdl> UpdateAsync(MbsSysDptLgcUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysDptLgcUpdateRspMdl
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

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 判斷 名稱 是否需要更新
        if (string.IsNullOrWhiteSpace(reqObj.ManagerDepartmentName) == false)
        {
            // 名稱需要更新，檢查名稱是否重複

            // 核心-管理者-部門-資料庫-是否存在
            var coMgrDptDbExistReqObj = new CoMgrDptDbExistReqMdl()
            {
                ManagerDepartmentName = reqObj.ManagerDepartmentName,
            };
            var coMgrDptDbExistRspObj = await this._coMgrDepartmentDb.ExistAsync(coMgrDptDbExistReqObj);
            if (coMgrDptDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrDptDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 名稱重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-部門-資料庫-更新
        var coMgrDptDbUpdateReqObj = new CoMgrDptDbUpdateReqMdl()
        {
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerDepartmentName = reqObj.ManagerDepartmentName,
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
        };
        var coMgrDptDbUpdateRspObj = await this._coMgrDepartmentDb.UpdateAsync(coMgrDptDbUpdateReqObj);
        if (coMgrDptDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

}