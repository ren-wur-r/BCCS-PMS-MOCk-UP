using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Product.Format;
using ServiceLibrary.Core.Manager.DB.Product.Service;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Service;

/// <summary>管理者後台-系統-產品-邏輯服務</summary>
public class MbsSysProductLogicalService : IMbsSysProductLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSysProductLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-產品-資料庫服務</summary>
    private readonly ICoMgrProductDbService _coMgrProductDb;

    /// <summary>核心-管理者-產品規格-資料庫服務</summary>
    private readonly ICoMgrProductSpecificationDbService _coMgrProductSpecificationDb;

    /// <summary>核心-管理者-產品主分類-資料庫服務</summary>
    private readonly ICoMgrProductMainKindDbService _coMgrProductMainKindDb;

    /// <summary>核心-管理者-產品子分類-資料庫服務</summary>
    private readonly ICoMgrProductSubKindDbService _coMgrProductSubKindDb;


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
    public MbsSysProductLogicalService(
    ILogger<MbsSysProductLogicalService> logger,
    // Core Manager
    ICoMgrProductDbService coMgrProductDb,
    ICoMgrProductSpecificationDbService coMgrProductSpecificationDb,
    ICoMgrProductMainKindDbService coMgrProductMainKindDb,
    ICoMgrProductSubKindDbService coMgrProductSubKindDb,
    // ManagerBackSite
    IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrProductDb = coMgrProductDb;
        this._coMgrProductSpecificationDb = coMgrProductSpecificationDb;
        this._coMgrProductMainKindDb = coMgrProductMainKindDb;
        this._coMgrProductSubKindDb = coMgrProductSubKindDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._atomMenu = DbAtomMenuEnum.SystemProduct;
    }

    #region Product 產品

    /// <summary>管理者後台-系統-產品-邏輯服務-取得多筆</summary>
    public async Task<MbsSysPrdLgcGetManyRspMdl> GetManyAsync(MbsSysPrdLgcGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-產品-資料庫-取得[筆數]從[管理者後台-系統設定-產品]
        var coMgrPrdSpcDbGetCountFromMbsSysPrdReqObj = new CoMgrPsDbGetCountFromMbsSysPrdReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductName = reqObj.ManagerProductName,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable,
        };
        var coMgrPrdSpcDbGetCountFromMbsSysPrdRspObj = await this._coMgrProductSpecificationDb.GetCountFromMbsSysPrdAsync(coMgrPrdSpcDbGetCountFromMbsSysPrdReqObj);
        if (coMgrPrdSpcDbGetCountFromMbsSysPrdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrPrdSpcDbGetCountFromMbsSysPrdRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerProductSpecificationList = new List<MbsSysPrdLgcGetManyItemRspMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-系統設定-產品]
        var coMgrPrdSpcDbGetManyFromMbsSysPrdReqObj = new CoMgrPsDbGetManyFromMbsSysPrdReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductName = reqObj.ManagerProductName,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coMgrPrdSpcDbGetManyFromMbsSysPrdRspObj = await this._coMgrProductSpecificationDb.GetManyFromMbsSysPrdAsync(coMgrPrdSpcDbGetManyFromMbsSysPrdReqObj);
        if (coMgrPrdSpcDbGetManyFromMbsSysPrdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductSpecificationList = coMgrPrdSpcDbGetManyFromMbsSysPrdRspObj.ManagerProductSpecificationList
            .Select(x => new MbsSysPrdLgcGetManyItemRspMdl()
            {
                ManagerProductID = x.ManagerProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductIsKey = x.ManagerProductIsKey,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable
            })
            .ToList();
        rspObj.TotalCount = coMgrPrdSpcDbGetCountFromMbsSysPrdRspObj.Count;
        return rspObj;

    }

    /// <summary>管理者後台-系統-產品-邏輯服務-新增</summary>
    public async Task<MbsSysPrdLgcAddRspMdl> AddAsync(MbsSysPrdLgcAddReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 新增產品時最少新增一筆規格
        if (reqObj.ManagerProductSpecificationList == null
            || reqObj.ManagerProductSpecificationList.Count == 0)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品-資料庫-是否存在
        var coMgrPrdDbExistReqObj = new CoMgrPrdDbExistReqMdl()
        {
            ManagerProductName = reqObj.ManagerProductName
        };
        var coMgrPrdDbExistRspObj = await this._coMgrProductDb.ExistAsync(coMgrPrdDbExistReqObj);
        if (coMgrPrdDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrPrdDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-產品-資料庫-新增
        var coMgrPrdDbAddReqObj = new CoMgrPrdDbAddReqMdl()
        {
            ManagerProductName = reqObj.ManagerProductName,
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductKind = reqObj.ManagerProductKind,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductRemark = reqObj.ManagerProductRemark,
            ManagerProductIsEnable = reqObj.ManagerProductIsEnable,
        };
        var coMgrPrdDbAddRspObj = await this._coMgrProductDb.AddAsync(coMgrPrdDbAddReqObj);
        if (coMgrPrdDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品規格-資料庫-新增多筆
        var coMgrPsDbAddManyReqObj = new CoMgrPsDbAddManyReqMdl()
        {
            ManagerProductSpecificationList = reqObj.ManagerProductSpecificationList
                .Select(x => new CoMgrPsDbAddManyReqItemMdl()
                {
                    ManagerProductID = coMgrPrdDbAddRspObj.ManagerProductID,
                    ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                    ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                    ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                    ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable,
                })
                .ToList()
        };
        var coMgrPsDbAddManyRspObj = await this._coMgrProductSpecificationDb.AddManyAsync(coMgrPsDbAddManyReqObj);
        if (coMgrPsDbAddManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // recover
            // db, 核心-管理者-產品-資料庫-移除
            var coMgrPrdDbRemoveReqObj = new CoMgrPrdDbRemoveReqMdl()
            {
                ManagerProductID = coMgrPrdDbAddRspObj.ManagerProductID,
            };
            var coMgrPrdDbRemoveRspObj = await this._coMgrProductDb.RemoveAsync(coMgrPrdDbRemoveReqObj);
            if (coMgrPrdDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            return rspObj;
        }
        if (coMgrPsDbAddManyRspObj.IsSuccess == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // recover
            // db, 核心-管理者-產品-資料庫-移除
            var coMgrPrdDbRemoveReqObj = new CoMgrPrdDbRemoveReqMdl()
            {
                ManagerProductID = coMgrPrdDbAddRspObj.ManagerProductID,
            };
            var coMgrPrdDbRemoveRspObj = await this._coMgrProductDb.RemoveAsync(coMgrPrdDbRemoveReqObj);
            if (coMgrPrdDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductID = coMgrPrdDbAddRspObj.ManagerProductID;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-更新</summary>
    public async Task<MbsSysPrdLgcUpdateRspMdl> UpdateAsync(MbsSysPrdLgcUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 判斷 名稱 是否需要更新
        if (!string.IsNullOrWhiteSpace(reqObj.ManagerProductName))
        {
            // 名稱需要更新，檢查名稱是否重複
            var coMgrPrdDbExistReqObj = new CoMgrPrdDbExistReqMdl()
            {
                ManagerProductID = reqObj.ManagerProductID,
                ManagerProductName = reqObj.ManagerProductName
            };
            var coMgrPrdDbExistRspObj = await this._coMgrProductDb.ExistAsync(coMgrPrdDbExistReqObj);
            if (coMgrPrdDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrPrdDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-產品-資料庫-更新
        var coMgrPrdDbUpdateReqObj = new CoMgrPrdDbUpdateReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductName = reqObj.ManagerProductName,
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductKind = reqObj.ManagerProductKind,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductRemark = reqObj.ManagerProductRemark,
            ManagerProductIsEnable = reqObj.ManagerProductIsEnable,
        };
        var coMgrPrdDbUpdateRspObj = await this._coMgrProductDb.UpdateAsync(coMgrPrdDbUpdateReqObj);
        if (coMgrPrdDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-取得</summary>
    public async Task<MbsSysPrdLgcGetRspMdl> GetAsync(MbsSysPrdLgcGetReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-產品-資料庫-取得
        var coMgrPrdDbGetReqObj = new CoMgrPrdDbGetReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID
        };
        var coMgrPrdDbGetRspObj = await this._coMgrProductDb.GetAsync(coMgrPrdDbGetReqObj);
        if (coMgrPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得
        var coMgrPmkDbGetReqObj = new CoMgrPmkDbGetReqMdl()
        {
            ManagerProductMainKindID = coMgrPrdDbGetRspObj.ManagerProductMainKindID
        };
        var coMgrPmkDbGetRspObj = await this._coMgrProductMainKindDb.GetAsync(coMgrPmkDbGetReqObj);
        if (coMgrPmkDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-取得
        var coMgrPskDbGetReqObj = new CoMgrPskDbGetReqMdl()
        {
            ManagerProductSubKindID = coMgrPrdDbGetRspObj.ManagerProductSubKindID
        };
        var coMgrPskDbGetRspObj = await this._coMgrProductSubKindDb.GetAsync(coMgrPskDbGetReqObj);
        if (coMgrPskDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        //db, 核心-管理者-產品規格-資料庫-取得多筆
        var coMgrPsDbGetManyReqObj = new CoMgrPsDbGetManyReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID
        };
        var coMgrPsDbGetManyRspObj = await this._coMgrProductSpecificationDb.GetManyAsync(coMgrPsDbGetManyReqObj);
        if (coMgrPsDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductID = coMgrPrdDbGetRspObj.ManagerProductID;
        rspObj.ManagerProductName = coMgrPrdDbGetRspObj.ManagerProductName;
        rspObj.ManagerProductMainKindID = coMgrPrdDbGetRspObj.ManagerProductMainKindID;
        rspObj.ManagerProductMainKindName = coMgrPmkDbGetRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductSubKindID = coMgrPrdDbGetRspObj.ManagerProductSubKindID;
        rspObj.ManagerProductSubKindName = coMgrPskDbGetRspObj.ManagerProductSubKindName;
        rspObj.ManagerProductKind = coMgrPrdDbGetRspObj.ManagerProductKind;
        rspObj.ManagerProductIsKey = coMgrPrdDbGetRspObj.ManagerProductIsKey;
        rspObj.ManagerProductRemark = coMgrPrdDbGetRspObj.ManagerProductRemark;
        rspObj.ManagerProductIsEnable = coMgrPrdDbGetRspObj.ManagerProductIsEnable;
        rspObj.ManagerProductSpecificationList = coMgrPsDbGetManyRspObj.ManagerProductSpecificationList
            .Select(x => new MbsSysPrdLgcGetManySpecificationRspItemMdl()
            {
                ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-新增規格</summary>
    public async Task<MbsSysPrdLgcAddSpecificationRspMdl> AddSpecificationAsync(MbsSysPrdLgcAddSpecificationReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcAddSpecificationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // db, 核心-管理者-產品規格-資料庫-是否存在
        var coMgrPrdSpcDbExistReqObj = new CoMgrPsDbExistReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
        };
        var coMgrPrdSpcDbExistRspObj = await this._coMgrProductSpecificationDb.ExistAsync(coMgrPrdSpcDbExistReqObj);
        if (coMgrPrdSpcDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrPrdSpcDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-產品規格-資料庫-新增
        var coMgrPrdSpcDbAddReqObj = new CoMgrPsDbAddReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            ManagerProductSpecificationSellPrice = reqObj.ManagerProductSpecificationSellPrice,
            ManagerProductSpecificationCostPrice = reqObj.ManagerProductSpecificationCostPrice,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable
        };
        var coMgrPrdSpcDbAddRspObj = await this._coMgrProductSpecificationDb.AddAsync(coMgrPrdSpcDbAddReqObj);
        if (coMgrPrdSpcDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;

    }

    /// <summary>管理者後台-系統-產品-邏輯服務-更新規格</summary>
    public async Task<MbsSysPrdLgcUpdateSpecificationRspMdl> UpdateSpecificationAsync(MbsSysPrdLgcUpdateSpecificationReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcUpdateSpecificationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // 判斷 名稱 是否需要更新
        if (!string.IsNullOrWhiteSpace(reqObj.ManagerProductSpecificationName))
        {
            // 名稱需要更新，檢查名稱是否重複

            // 核心-管理者-產品規格-資料庫-是否存在
            var coMgrPrdSpcDbExistReqObj = new CoMgrPsDbExistReqMdl()
            {
                ManagerProductID = reqObj.ManagerProductID,
                ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
                ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            };
            var coMgrPrdSpcDbExistRspObj = await this._coMgrProductSpecificationDb.ExistAsync(coMgrPrdSpcDbExistReqObj);
            if (coMgrPrdSpcDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrPrdSpcDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 名稱重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-產品規格-資料庫-更新
        var coMgrPrdSpcDbUpdateReqObj = new CoMgrPsDbUpdateReqMdl()
        {
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            ManagerProductSpecificationSellPrice = reqObj.ManagerProductSpecificationSellPrice,
            ManagerProductSpecificationCostPrice = reqObj.ManagerProductSpecificationCostPrice,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable
        };
        var coMgrPrdSpcDbUpdateRspObj = await this._coMgrProductSpecificationDb.UpdateAsync(coMgrPrdSpcDbUpdateReqObj);
        if (coMgrPrdSpcDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region ProductMainKind 產品主分類

    /// <summary>管理者後台-系統-產品-邏輯服務-取得多筆主分類</summary>
    public async Task<MbsSysPrdLgcGetManyMainKindRspMdl> GetManyMainKindAsync(MbsSysPrdLgcGetManyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcGetManyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // db, 核心-管理者-產品主分類-資料庫-取得[筆數]從[管理者後台-系統-產品]
        var coMgrPmkDbGetCountFromMbsSysPrdReqObj = new CoMgrPmkDbGetCountFromMbsSysPrdReqMdl()
        {
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable
        };
        var coMgrPmkDbGetCountFromMbsSysPrdRspObj = await this._coMgrProductMainKindDb.GetCountFromMbsSysPrdAsync(coMgrPmkDbGetCountFromMbsSysPrdReqObj);
        if (coMgrPmkDbGetCountFromMbsSysPrdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrPmkDbGetCountFromMbsSysPrdRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerProductMainKindList = new List<MbsSysPrdLgcGetManyMainKindRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-系統-產品-主分類]
        var coMgrPmkDbGetManyFromMbsSysPrdReqObj = new CoMgrPmkDbGetManyFromMbsSysPrdReqMdl()
        {
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrPmkDbGetManyFromMbsSysPrdRspObj = await this._coMgrProductMainKindDb.GetManyFromMbsSysPrdAsync(coMgrPmkDbGetManyFromMbsSysPrdReqObj);
        if (coMgrPmkDbGetManyFromMbsSysPrdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindList = coMgrPmkDbGetManyFromMbsSysPrdRspObj.ManagerProductMainKindList
            .Select(x => new MbsSysPrdLgcGetManyMainKindRspItemMdl()
            {
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductMainKindCommissionRate = x.ManagerProductMainKindCommissionRate,
                ManagerProductMainKindIsEnable = x.ManagerProductMainKindIsEnable
            })
            .ToList();
        rspObj.TotalCount = coMgrPmkDbGetCountFromMbsSysPrdRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-新增主分類</summary>
    public async Task<MbsSysPrdLgcAddMainKindRspMdl> AddMainKindAsync(MbsSysPrdLgcAddMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcAddMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // db, 核心-管理者-產品主分類-資料庫-是否存在
        var CoMgrPmkDbExistReqObj = new CoMgrPmkDbExistReqMdl()
        {
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName
        };
        var CoMgrPmkDbExistRspObj = await this._coMgrProductMainKindDb.ExistAsync(CoMgrPmkDbExistReqObj);
        if (CoMgrPmkDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (CoMgrPmkDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 名稱重複
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-新增
        var CoMgrPmkDbAddReqObj = new CoMgrPmkDbAddReqMdl()
        {
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName,
            ManagerProductMainKindCommissionRate = reqObj.ManagerProductMainKindCommissionRate,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable
        };
        var CoMgrPmkDbAddRspObj = await this._coMgrProductMainKindDb.AddAsync(CoMgrPmkDbAddReqObj);
        if (CoMgrPmkDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-更新主分類</summary>
    public async Task<MbsSysPrdLgcUpdateMainKindRspMdl> UpdateMainKindAsync(MbsSysPrdLgcUpdateMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcUpdateMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // 判斷名稱是否需要更新
        if (!string.IsNullOrWhiteSpace(reqObj.ManagerProductMainKindName))

        {
            // 名稱需要更新，檢查名稱是否重複

            // db, 核心-管理者-產品主分類-資料庫-是否存在
            var CoMgrPmkDbExistReqObj = new CoMgrPmkDbExistReqMdl()
            {
                ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
                ManagerProductMainKindName = reqObj.ManagerProductMainKindName
            };
            var CoMgrPmkDbExistRspObj = await this._coMgrProductMainKindDb.ExistAsync(CoMgrPmkDbExistReqObj);
            if (CoMgrPmkDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (CoMgrPmkDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 名稱重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-產品主分類-資料庫-更新
        var CoMgrPmkDbUpdateReqObj = new CoMgrPmkDbUpdateReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName,
            ManagerProductMainKindCommissionRate = reqObj.ManagerProductMainKindCommissionRate,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable
        };
        var CoMgrPmkDbUpdateRspObj = await this._coMgrProductMainKindDb.UpdateAsync(CoMgrPmkDbUpdateReqObj);
        if (CoMgrPmkDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-取得主分類</summary>
    public async Task<MbsSysPrdLgcGetMainKindRspMdl> GetMainKindAsync(MbsSysPrdLgcGetMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcGetMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // db, 核心-管理者-產品主分類-資料庫-取得
        var CoMgrPmkDbGetReqObj = new CoMgrPmkDbGetReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
        };
        var CoMgrPmkDbGetRspObj = await this._coMgrProductMainKindDb.GetAsync(CoMgrPmkDbGetReqObj);
        if (CoMgrPmkDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindName = CoMgrPmkDbGetRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductMainKindCommissionRate = CoMgrPmkDbGetRspObj.ManagerProductMainKindCommissionRate;
        rspObj.ManagerProductMainKindIsEnable = CoMgrPmkDbGetRspObj.ManagerProductMainKindIsEnable;
        return rspObj;
    }

    #endregion

    #region ProductSubKind 產品子分類

    /// <summary>管理者後台-系統-產品-邏輯服務-取得多筆子分類</summary>
    public async Task<MbsSysPrdLgcGetManySubKindRspMdl> GetManySubKindAsync(MbsSysPrdLgcGetManySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcGetManySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // db, 核心-管理者-產品子分類-資料庫-取得[筆數]從[管理者後台-系統-產品-子分類]
        var coMgrPskDbGetCountFromMbsSysPrdReqObj = new CoMgrPskDbGetCountFromMbsSysPrdReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable,
        };
        var coMgrPskDbGetCountFromMbsSysPrdRspObj = await this._coMgrProductSubKindDb.GetCountFromMbsSysPrdAsync(coMgrPskDbGetCountFromMbsSysPrdReqObj);
        if (coMgrPskDbGetCountFromMbsSysPrdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrPskDbGetCountFromMbsSysPrdRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerProductSubKindList = new List<MbsSysPrdLgcGetManySubKindRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-系統-產品-子分類]
        var coMgrPskDbGetManyFromMbsSysPrdReqObj = new CoMgrPskDbGetManyFromMbsSysPrdReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrPskDbGetManyFromMbsSysPrdRspObj = await this._coMgrProductSubKindDb.GetManyFromMbsSysPrdAsync(coMgrPskDbGetManyFromMbsSysPrdReqObj);
        if (coMgrPskDbGetManyFromMbsSysPrdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductSubKindList = coMgrPskDbGetManyFromMbsSysPrdRspObj.ManagerProductSubKindList
            .Select(x => new MbsSysPrdLgcGetManySubKindRspItemMdl
            {
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductSubKindCommissionRate = x.ManagerProductSubKindCommissionRate,
                ManagerProductSubKindIsEnable = x.ManagerProductSubKindIsEnable
            })
            .ToList();
        rspObj.TotalCount = coMgrPskDbGetCountFromMbsSysPrdRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-新增子分類</summary>
    public async Task<MbsSysPrdLgcAddSubKindRspMdl> AddSubKindAsync(MbsSysPrdLgcAddSubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcAddSubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        // db, 核心-管理者-產品子分類-資料庫-是否存在
        var CoMgrPskDbExistReqObj = new CoMgrPskDbExistReqMdl()
        {
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName
        };
        var CoMgrPskDbExistRspObj = await this._coMgrProductSubKindDb.ExistAsync(CoMgrPskDbExistReqObj);
        if (CoMgrPskDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (CoMgrPskDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 名稱重複
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-新增
        var CoMgrPskDbAddReqObj = new CoMgrPskDbAddReqMdl()
        {
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName,
            ManagerProductSubKindCommissionRate = reqObj.ManagerProductSubKindCommissionRate,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable
        };
        var CoMgrPskDbAddRspObj = await this._coMgrProductSubKindDb.AddAsync(CoMgrPskDbAddReqObj);
        if (CoMgrPskDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-更新子分類</summary>
    public async Task<MbsSysPrdLgcUpdateSubKindRspMdl> UpdateSubKindAsync(MbsSysPrdLgcUpdateSubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcUpdateSubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu
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

        //判斷 名稱 是否需要更新
        if (string.IsNullOrWhiteSpace(reqObj.ManagerProductSubKindName) == false)
        {
            // 名稱需要更新，檢查名稱是否重複

            // db, 核心-管理者-產品子分類-資料庫-是否存在
            var CoMgrPskDbExistReqObj = new CoMgrPskDbExistReqMdl()
            {
                ManagerProductSubKindName = reqObj.ManagerProductSubKindName
            };
            var CoMgrPskDbExistRspObj = await this._coMgrProductSubKindDb.ExistAsync(CoMgrPskDbExistReqObj);
            if (CoMgrPskDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (CoMgrPskDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 名稱重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-產品子分類-資料庫-更新
        var CoMgrPskDbUpdateReqObj = new CoMgrPskDbUpdateReqMdl()
        {
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName,
            ManagerProductSubKindCommissionRate = reqObj.ManagerProductSubKindCommissionRate,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable
        };
        var CoMgrPskDbUpdateRspObj = await this._coMgrProductSubKindDb.UpdateAsync(CoMgrPskDbUpdateReqObj);
        if (CoMgrPskDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統-產品-邏輯服務-取得子分類</summary>
    public async Task<MbsSysPrdLgcGetSubKindRspMdl> GetSubKindAsync(MbsSysPrdLgcGetSubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdLgcGetSubKindRspMdl
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

        // db, 核心-管理者-產品子分類-資料庫-取得
        var coMgrPskDbGetReqObj = new CoMgrPskDbGetReqMdl()
        {
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID
        };
        var coMgrPskDbGetRspObj = await this._coMgrProductSubKindDb.GetAsync(coMgrPskDbGetReqObj);
        if (coMgrPskDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得
        var coMgrPmkDbGetReqObj = new CoMgrPmkDbGetReqMdl()
        {
            ManagerProductMainKindID = coMgrPskDbGetRspObj.ManagerProductMainKindID
        };
        var coMgrPmkDbGetRspObj = await this._coMgrProductMainKindDb.GetAsync(coMgrPmkDbGetReqObj);
        if (coMgrPmkDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindName = coMgrPmkDbGetRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductSubKindName = coMgrPskDbGetRspObj.ManagerProductSubKindName;
        rspObj.ManagerProductSubKindCommissionRate = coMgrPskDbGetRspObj.ManagerProductSubKindCommissionRate;
        rspObj.ManagerProductSubKindIsEnable = coMgrPskDbGetRspObj.ManagerProductSubKindIsEnable;
        return rspObj;
    }

    #endregion

}