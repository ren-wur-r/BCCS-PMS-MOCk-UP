using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomPermissionKind;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Region.Format;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Service;

/// <summary>管理者後台-系統設定-地區-邏輯服務</summary>
public class MbsSysRegionLogicalService : IMbsSysRegionLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsSysRegionLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-地區-資料庫服務</summary>
    private readonly ICoMgrRegionDbService _coMgrRegionDb;

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
    public MbsSysRegionLogicalService(
        ILogger<MbsSysRegionLogicalService> logger,
        // Core Manager
        ICoMgrRegionDbService coMgrRegionDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrRegionDb = coMgrRegionDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._AtomMenu = DataModelLibrary.Database.AtomMenu.DbAtomMenuEnum.SystemRegion;
    }

    /// <summary>管理者後台-系統設定-地區-邏輯-取得多筆</summary>
    public async Task<MbsSysRgnLgcGetManyRspMdl> GetManyAsync(MbsSysRgnLgcGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnLgcGetManyRspMdl
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

        // db, 核心-管理者-地區-資料庫-取得[筆數]從[管理者後台-系統-地區]
        var coMgrRgnDbGetCountFromMbsSysRgnReqObj = new CoMgrRgnDbGetCountFromMbsSysRgnReqMdl()
        {
            ManagerRegionName = reqObj.ManagerRegionName,
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
        };
        var coMgrRgnDbGetCountFromMbsSysRgnRspObj = await this._coMgrRegionDb.GetCountFromMbsSysRgnAsync(coMgrRgnDbGetCountFromMbsSysRgnReqObj);
        if (coMgrRgnDbGetCountFromMbsSysRgnRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrRgnDbGetCountFromMbsSysRgnRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerRegionList = new List<MbsSysRgnLgcGetManyItemRspMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-地區-資料庫-取得多筆從[管理者後台-系統-地區]
        var coMgrRgnDbGetManyFromMbsSysRgnReqObj = new CoMgrRgnDbGetManyFromMbsSysRgnReqMdl()
        {
            ManagerRegionName = reqObj.ManagerRegionName,
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coMgrRgnDbGetManyFromMbsSysRgnRspObj = await this._coMgrRegionDb.GetManyFromMbsSysRgnAsync(coMgrRgnDbGetManyFromMbsSysRgnReqObj);
        if (coMgrRgnDbGetManyFromMbsSysRgnRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRegionList = coMgrRgnDbGetManyFromMbsSysRgnRspObj.ManagerRegionList
            .Select(x => new MbsSysRgnLgcGetManyItemRspMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerRegionIsEnable = x.ManagerRegionIsEnable,
            })
            .ToList();
        rspObj.TotalCount = coMgrRgnDbGetCountFromMbsSysRgnRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-地區-邏輯-取得</summary>
    public async Task<MbsSysRgnLgcGetRspMdl> GetAsync(MbsSysRgnLgcGetReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnLgcGetRspMdl
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

        // db, 核心-管理者-地區-資料庫-取得
        var coMgrRgnDbGetReqObj = new CoMgrRgnDbGetReqMdl()
        {
            ManagerRegionID = reqObj.ManagerRegionID,
        };
        var coMgrRgnDbGetRspObj = await this._coMgrRegionDb.GetAsync(coMgrRgnDbGetReqObj);
        if (coMgrRgnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRegionName = coMgrRgnDbGetRspObj.ManagerRegionName;
        rspObj.ManagerRegionIsEnable = coMgrRgnDbGetRspObj.ManagerRegionIsEnable;

        return rspObj;
    }

    /// <summary>管理者後台-系統設定-地區-邏輯-新增</summary>
    public async Task<MbsSysRgnLgcAddRspMdl> AddAsync(MbsSysRgnLgcAddReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnLgcAddRspMdl
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

        // 核心-管理者-地區-資料庫-是否存在
        var coMgrRgnDbExistReqObj = new CoMgrRgnDbExistReqMdl()
        {
            ManagerRegionName = reqObj.ManagerRegionName,
        };
        var coMgrRgnDbExistRspObj = await this._coMgrRegionDb.ExistAsync(coMgrRgnDbExistReqObj);
        if (coMgrRgnDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrRgnDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 名稱重複
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-地區-資料庫-新增
        var coMgrRgnDbAddReqObj = new CoMgrRgnDbAddReqMdl()
        {
            ManagerRegionName = reqObj.ManagerRegionName,
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
        };
        var coMgrRgnDbAddRspObj = await this._coMgrRegionDb.AddAsync(coMgrRgnDbAddReqObj);
        if (coMgrRgnDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-地區-邏輯-更新</summary>
    public async Task<MbsSysRgnLgcUpdateRspMdl> UpdateAsync(MbsSysRgnLgcUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnLgcUpdateRspMdl
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
        if (string.IsNullOrWhiteSpace(reqObj.ManagerRegionName) == false)
        {
            // 名稱需要更新，檢查名稱是否重複

            // 核心-管理者-地區-資料庫-是否存在
            var coMgrRgnDbExistReqObj = new CoMgrRgnDbExistReqMdl()
            {
                ManagerRegionName = reqObj.ManagerRegionName,
            };
            var coMgrRgnDbExistRspObj = await this._coMgrRegionDb.ExistAsync(coMgrRgnDbExistReqObj);
            if (coMgrRgnDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrRgnDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 名稱重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-地區-資料庫-更新
        var coMgrRgnDbUpdateReqObj = new CoMgrRgnDbUpdateReqMdl()
        {
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerRegionName = reqObj.ManagerRegionName,
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
        };
        var coMgrRgnDbUpdateRspObj = await this._coMgrRegionDb.UpdateAsync(coMgrRgnDbUpdateReqObj);
        if (coMgrRgnDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

}
