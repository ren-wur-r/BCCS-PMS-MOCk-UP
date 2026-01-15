using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Company.Format;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;
using ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Service;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Service;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Service;

/// <summary>管理者後台-系統設定-客戶-邏輯服務</summary>
public class MbsSysCmpLogicalService : IMbsSysCmpLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsSysCmpLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-關係公司-資料庫服務</summary>
    public readonly ICoMgrCompanyAffiliateDbService _coMgrAffiliateCompanyDb;

    /// <summary>核心-管理者-公司-資料庫服務</summary>
    public readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    /// <summary>核心-管理者-公司營業地點-資料庫服務</summary>
    public readonly ICoMgrCompanyLocationDbService _coMgrCompanyLocationDb;

    /// <summary>核心-管理者-公司-主分類-資料庫服務</summary>
    public readonly ICoMgrCompanyMainKindDbService _coMgrCompanyMainKindDb;

    /// <summary>核心-管理者-公司-子分類-資料庫服務</summary>
    public readonly ICoMgrCompanySubKindDbService _coMgrCompanySubKindDb;

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
    public MbsSysCmpLogicalService(
    ILogger<MbsSysCmpLogicalService> logger,
    // Core Manager
    ICoMgrCompanyAffiliateDbService coMgrAffiliateCompanyDb,
    ICoMgrCompanyDbService coMgrCompanyDb,
    ICoMgrCompanyLocationDbService coMgrCompanyLocationDb,
    ICoMgrCompanyMainKindDbService coMgrCompanyMainKindDb,
    ICoMgrCompanySubKindDbService coMgrCompanySubKindDb,
    // ManagerBackSite
    IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrAffiliateCompanyDb = coMgrAffiliateCompanyDb;
        this._coMgrCompanyDb = coMgrCompanyDb;
        this._coMgrCompanyLocationDb = coMgrCompanyLocationDb;
        this._coMgrCompanyMainKindDb = coMgrCompanyMainKindDb;
        this._coMgrCompanySubKindDb = coMgrCompanySubKindDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._AtomMenu = DbAtomMenuEnum.SystemCompany;
    }

    #region 公司主分類

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司主分類</summary>
    public async Task<MbsSysCmpLgcGetManyCompanyMainKindRspMdl> GetManyCompanyMainKindAsync(MbsSysCmpLgcGetManyCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetManyCompanyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Success
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

        // db, 核心-管理者-公司主分類-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]
        var coMgrCmpMainKdDbGetCountFrommbsSysCmpReqObj = new CoMgrCmpMainKdDbGetCountFromMbsSysCmpReqMdl()
        {
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable,
        };
        var coMgrCmpMainKdDbGetCountFrommbsSysCmpRspObj = await this._coMgrCompanyMainKindDb.GetCountFromMbsSysCmpAsync(coMgrCmpMainKdDbGetCountFrommbsSysCmpReqObj);
        if (coMgrCmpMainKdDbGetCountFrommbsSysCmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrCmpMainKdDbGetCountFrommbsSysCmpRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerCompanyMainKindList = new List<MbsSysCmpLgcGetManyCompanyMainKindRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]
        var coMgrCmpMainKdDbGetManyFrommbsSysCmpReqObj = new CoMgrCmpMainKdDbGetManyFromMbsSysCmpReqMdl()
        {
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrCmpMainKdDbGetManyFrommbsSysCmpRspObj = await this._coMgrCompanyMainKindDb.GetManyFromMbsSysCmpAsync(coMgrCmpMainKdDbGetManyFrommbsSysCmpReqObj);
        if (coMgrCmpMainKdDbGetManyFrommbsSysCmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyMainKindList = coMgrCmpMainKdDbGetManyFrommbsSysCmpRspObj.ManagerCompanyMainKindList?
            .Select(x => new MbsSysCmpLgcGetManyCompanyMainKindRspItemMdl()
            {
                ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                ManagerCompanyMainKindName = x.ManagerCompanyMainKindName,
                ManagerCompanyMainKindIsEnable = x.ManagerCompanyMainKindIsEnable
            }).ToList();
        rspObj.TotalCount = coMgrCmpMainKdDbGetCountFrommbsSysCmpRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得單筆公司主分類</summary>
    public async Task<MbsSysCmpLgcGetCompanyMainKindsRspMdl> GetCompanyMainKindAsync(MbsSysCmpLgcGetCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetCompanyMainKindsRspMdl
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

        // db, 核心-管理者-公司主分類-資料庫-取得
        var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID
        };
        var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
        if (coMgrCmpMainKdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyMainKindName = coMgrCmpMainKdDbGetRspObj.ManagerCompanyMainKindName;
        rspObj.ManagerCompanyMainKindIsEnable = coMgrCmpMainKdDbGetRspObj.ManagerCompanyMainKindIsEnable;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司主分類</summary>
    public async Task<MbsSysCmpLgcAddCompanyMainKindRspMdl> AddCompanyMainKindAsync(MbsSysCmpLgcAddCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcAddCompanyMainKindRspMdl
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

        // db, 核心-管理者-公司主分類-資料庫-檢查名稱重複
        var coMgrCmpMainKdDbCheckNameDuplicateReqObj = new CoMgrCmpMainKdDbCheckNameDuplicateReqMdl()
        {
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
        };
        var coMgrCmpMainKdDbCheckNameDuplicateRspObj = await this._coMgrCompanyMainKindDb.CheckNameDuplicateAsync(coMgrCmpMainKdDbCheckNameDuplicateReqObj);
        if (coMgrCmpMainKdDbCheckNameDuplicateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrCmpMainKdDbCheckNameDuplicateRspObj.IsDuplicate)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-新增
        var coMgrCmpMainKdDbAddReqObj = new CoMgrCmpMainKdDbAddReqMdl()
        {
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable
        };
        var coMgrCmpMainKdDbAddRspObj = await this._coMgrCompanyMainKindDb.AddAsync(coMgrCmpMainKdDbAddReqObj);
        if (coMgrCmpMainKdDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyMainKindID = coMgrCmpMainKdDbAddRspObj.ManagerCompanyMainKindID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-更新公司主分類</summary>
    public async Task<MbsSysCmpLgcUpdateCompanyMainKindRspMdl> UpdateCompanyMainKindAsync(MbsSysCmpLgcUpdateCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcUpdateCompanyMainKindRspMdl
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

        // 檢查名稱是否重複
        if (!string.IsNullOrWhiteSpace(reqObj.ManagerCompanyMainKindName))
        {
            // db, 核心-管理者-公司主分類-資料庫-檢查名稱重複
            var coMgrCmpMainKdDbCheckNameDuplicateReqObj = new CoMgrCmpMainKdDbCheckNameDuplicateReqMdl()
            {
                ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
                ExcludeManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID
            };
            var coMgrCmpMainKdDbCheckNameDuplicateRspObj = await this._coMgrCompanyMainKindDb.CheckNameDuplicateAsync(coMgrCmpMainKdDbCheckNameDuplicateReqObj);
            if (coMgrCmpMainKdDbCheckNameDuplicateRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrCmpMainKdDbCheckNameDuplicateRspObj.IsDuplicate)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-公司主分類-資料庫-更新
        var coMgrCmpMainKdDbUpdateReqObj = new CoMgrCmpMainKdDbUpdateReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName,
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable
        };
        var coMgrCmpMainKdDbUpdateRspObj = await this._coMgrCompanyMainKindDb.UpdateAsync(coMgrCmpMainKdDbUpdateReqObj);
        if (coMgrCmpMainKdDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 公司子分類

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司子分類</summary>
    public async Task<MbsSysCmpLgcGetManyCompanySubKindRspMdl> GetManyCompanySubKindAsync(MbsSysCmpLgcGetManyCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetManyCompanySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Success
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

        // db, 核心-管理者-公司子分類-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]
        var coMgrCmpSubKdDbGetCountFrommbsSysCmpReqObj = new CoMgrCmpSubKdDbGetCountFromMbsSysCmpReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName,
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
        };
        var coMgrCmpSubKdDbGetCountFrommbsSysCmpRspObj = await this._coMgrCompanySubKindDb.GetCountFromMbsSysCmpAsync(coMgrCmpSubKdDbGetCountFrommbsSysCmpReqObj);
        if (coMgrCmpSubKdDbGetCountFrommbsSysCmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrCmpSubKdDbGetCountFrommbsSysCmpRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerCompanySubKindList = new List<MbsSysCmpLgcGetManyCompanySubKindRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]
        var coMgrCmpSubKdDbGetManyFrommbsSysCmpReqObj = new CoMgrCmpSubKdDbGetManyFromMbsSysCmpReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName,
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrCmpSubKdDbGetManyFrommbsSysCmpRspObj = await this._coMgrCompanySubKindDb.GetManyFromMbsSysCmpAsync(coMgrCmpSubKdDbGetManyFrommbsSysCmpReqObj);
        if (coMgrCmpSubKdDbGetManyFrommbsSysCmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得多筆[名稱]
        var coMgrCmpMainKdDbGetManyNameReqObj = new CoMgrCmpMainKdDbGetManyNameReqMdl()
        {
            ManagerCompanyMainKindIdList = coMgrCmpSubKdDbGetManyFrommbsSysCmpRspObj.ManagerCompanySubKindList
                                        .Select(x => x.ManagerCompanyMainKindID)
                                        .Distinct()
                                        .ToList()
        };
        var coMgrCmpMainKdDbGetManyNameRspObj = await this._coMgrCompanyMainKindDb.GetManyNameAsync(coMgrCmpMainKdDbGetManyNameReqObj);
        if (coMgrCmpMainKdDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanySubKindList =
        (
            from subKind in coMgrCmpSubKdDbGetManyFrommbsSysCmpRspObj.ManagerCompanySubKindList
            from mainKind in coMgrCmpMainKdDbGetManyNameRspObj.ManagerCompanyMainKindList
                                .Where(x => x.ManagerCompanyMainKindID == subKind.ManagerCompanyMainKindID)
                                .DefaultIfEmpty()
            select new MbsSysCmpLgcGetManyCompanySubKindRspItemMdl()
            {
                ManagerCompanySubKindID = subKind.ManagerCompanySubKindID,
                ManagerCompanySubKindName = subKind.ManagerCompanySubKindName,
                ManagerCompanyMainKindID = subKind.ManagerCompanyMainKindID,
                ManagerCompanyMainKindName = mainKind?.ManagerCompanyMainKindName,
                ManagerCompanySubKindIsEnable = subKind.ManagerCompanySubKindIsEnable
            }
        )
        .ToList();
        rspObj.TotalCount = coMgrCmpSubKdDbGetCountFrommbsSysCmpRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得單筆公司子分類</summary>
    public async Task<MbsSysCmpLgcGetCompanySubKindRspMdl> GetCompanySubKindAsync(MbsSysCmpLgcGetCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetCompanySubKindRspMdl
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

        // db, 核心-管理者-公司子分類-資料庫-取得
        var coMgrCmpSubKdDbGetReqObj = new CoMgrCmpSubKdDbGetReqMdl()
        {
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID
        };
        var coMgrCmpSubKdDbGetRspObj = await this._coMgrCompanySubKindDb.GetAsync(coMgrCmpSubKdDbGetReqObj);
        if (coMgrCmpSubKdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得
        var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
        {
            ManagerCompanyMainKindID = coMgrCmpSubKdDbGetRspObj.ManagerCompanyMainKindID
        };
        var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
        if (coMgrCmpMainKdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanySubKindName = coMgrCmpSubKdDbGetRspObj.ManagerCompanySubKindName;
        rspObj.ManagerCompanyMainKindID = coMgrCmpSubKdDbGetRspObj.ManagerCompanyMainKindID;
        rspObj.ManagerCompanySubKindIsEnable = coMgrCmpSubKdDbGetRspObj.ManagerCompanySubKindIsEnable;
        rspObj.ManagerCompanyMainKindName = coMgrCmpMainKdDbGetRspObj.ManagerCompanyMainKindName;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司子分類</summary>
    public async Task<MbsSysCmpLgcAddCompanySubKindRspMdl> AddCompanySubKindAsync(MbsSysCmpLgcAddCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcAddCompanySubKindRspMdl
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

        // db, 核心-管理者-公司主分類-資料庫-取得
        var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID
        };
        var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
        if (coMgrCmpMainKdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-新增
        var coMgrCmpSubKdDbAddReqObj = new CoMgrCmpSubKdDbAddReqMdl()
        {
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable
        };
        var coMgrCmpSubKdDbAddRspObj = await this._coMgrCompanySubKindDb.AddAsync(coMgrCmpSubKdDbAddReqObj);
        if (coMgrCmpSubKdDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanySubKindID = coMgrCmpSubKdDbAddRspObj.ManagerCompanySubKindID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-更新公司子分類</summary>
    public async Task<MbsSysCmpLgcUpdateCompanySubKindRspMdl> UpdateCompanySubKindAsync(MbsSysCmpLgcUpdateCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcUpdateCompanySubKindRspMdl
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

        // db, 核心-管理者-公司子分類-資料庫-更新
        var coMgrCmpSubKdDbUpdateReqObj = new CoMgrCmpSubKdDbUpdateReqMdl()
        {
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName,
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable
        };
        var coMgrCmpSubKdDbUpdateRspObj = await this._coMgrCompanySubKindDb.UpdateAsync(coMgrCmpSubKdDbUpdateReqObj);
        if (coMgrCmpSubKdDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 公司

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司</summary>
    public async Task<MbsSysCmpLgcGetManyCompanyRspMdl> GetManyCompanyAsync(MbsSysCmpLgcGetManyCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetManyCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Success
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

        // db, 核心-管理者-公司-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]
        var coMgrCmpDbGetCountFrommbsSysCmpReqObj = new CoMgrCmpDbGetCountFromMbsSysCmpReqMdl()
        {
            ManagerCompanyName = reqObj.ManagerCompanyName,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
        };
        var coMgrCmpDbGetCountFrommbsSysCmpRspObj = await this._coMgrCompanyDb.GetCountFromMbsSysCmpAsync(coMgrCmpDbGetCountFrommbsSysCmpReqObj);
        if (coMgrCmpDbGetCountFrommbsSysCmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrCmpDbGetCountFrommbsSysCmpRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerCompanyList = new List<MbsSysCmpLgcGetManyCompanyRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得多筆從[管理者後台-系統設定-客戶]
        var coMgrCmpDbGetManyFrommbsSysCmpReqObj = new CoMgrCmpDbGetManyFromMbsSysCmpReqMdl()
        {
            ManagerCompanyName = reqObj.ManagerCompanyName,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrCmpDbGetManyFrommbsSysCmpRspObj = await this._coMgrCompanyDb.GetManyFromMbsSysCmpAsync(coMgrCmpDbGetManyFrommbsSysCmpReqObj);
        if (coMgrCmpDbGetManyFrommbsSysCmpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得多筆[名稱]
        var coMgrCmpMainKdDbGetManyNameReqObj = new CoMgrCmpMainKdDbGetManyNameReqMdl()
        {
            ManagerCompanyMainKindIdList = coMgrCmpDbGetManyFrommbsSysCmpRspObj.ManagerCompanyList
                .Select(x => x.ManagerCompanyMainKindID)
                .Distinct()
                .ToList()
        };
        var coMgrCmpMainKdDbGetManyNameRspObj = await this._coMgrCompanyMainKindDb.GetManyNameAsync(coMgrCmpMainKdDbGetManyNameReqObj);
        if (coMgrCmpMainKdDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-取得多筆[名稱]
        var coMgrCmpSubKdDbGetManyByNameReqObj = new CoMgrCmpSubKdDbGetManyNameReqMdl()
        {
            ManagerCompanySubKindIdList = coMgrCmpDbGetManyFrommbsSysCmpRspObj.ManagerCompanyList
                .Select(x => x.ManagerCompanySubKindID)
                .Distinct()
                .ToList()
        };
        var coMgrCmpSubKdDbGetManyByNameRspObj = await this._coMgrCompanySubKindDb.GetManyNameAsync(coMgrCmpSubKdDbGetManyByNameReqObj);
        if (coMgrCmpSubKdDbGetManyByNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyList =
        (
            from company in coMgrCmpDbGetManyFrommbsSysCmpRspObj.ManagerCompanyList
            from mainKind in coMgrCmpMainKdDbGetManyNameRspObj.ManagerCompanyMainKindList
                .Where(x => x.ManagerCompanyMainKindID == company.ManagerCompanyMainKindID)
                .DefaultIfEmpty()
            from subKind in coMgrCmpSubKdDbGetManyByNameRspObj.ManagerCompanySubKindList
                .Where(x => x.ManagerCompanySubKindID == company.ManagerCompanySubKindID)
                .DefaultIfEmpty()
            select new MbsSysCmpLgcGetManyCompanyRspItemMdl()
            {
                ManagerCompanyID = company.ManagerCompanyID,
                ManagerCompanyUnifiedNumber = company.ManagerCompanyUnifiedNumber,
                ManagerCompanyName = company.ManagerCompanyName,
                AtomCustomerGrade = company.AtomCustomerGrade,
                ManagerCompanyMainKindName = mainKind?.ManagerCompanyMainKindName,
                ManagerCompanySubKindName = subKind?.ManagerCompanySubKindName
            }
        )
        .ToList();
        rspObj.TotalCount = coMgrCmpDbGetCountFrommbsSysCmpRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司</summary>
    public async Task<MbsSysCmpLgcGetCompanyRspMdl> GetCompanyAsync(MbsSysCmpLgcGetCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetCompanyRspMdl
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

        // db, 核心-管理者-公司-資料庫-取得
        var coMgrCmpDbGetReqObj = new CoMgrCmpDbGetReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID
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
        rspObj.ManagerCompanyUnifiedNumber = coMgrCmpDbGetRspObj.ManagerCompanyUnifiedNumber?.Trim();
        rspObj.ManagerCompanyName = coMgrCmpDbGetRspObj.ManagerCompanyName?.Trim();
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

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司</summary>
    public async Task<MbsSysCmpLgcAddCompanyRspMdl> AddCompanyAsync(MbsSysCmpLgcAddCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcAddCompanyRspMdl
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

        // db, 核心-管理者-公司-資料庫-統一編號是否存在
        var coMgrComDbUniNumExistReqObj = new CoMgrComDbUniNumExistReqMdl()
        {
            UnifiedNumber = reqObj.ManagerCompanyUnifiedNumber,
        };
        var coMgrComDbUniNumExistRspObj = await this._coMgrCompanyDb.UniNumExistAsync(coMgrComDbUniNumExistReqObj);
        if (coMgrComDbUniNumExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrComDbUniNumExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 統一編號重複
            rspObj.ErrorCode = MbsErrorCodeEnum.UnifiedNumberDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-公司主分類-資料庫-取得
        var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
        {
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID
        };
        var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
        if (coMgrCmpMainKdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-取得
        var coMgrCmpSubKdDbGetReqObj = new CoMgrCmpSubKdDbGetReqMdl()
        {
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID
        };
        var coMgrCmpSubKdDbGetRspObj = await this._coMgrCompanySubKindDb.GetAsync(coMgrCmpSubKdDbGetReqObj);
        if (coMgrCmpSubKdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-新增
        var coMgrCmpDbAddReqObj = new CoMgrCmpDbAddReqMdl()
        {
            ManagerCompanyUnifiedNumber = reqObj.ManagerCompanyUnifiedNumber?.Trim(),
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            AtomManagerCompanyStatus = reqObj.AtomManagerCompanyStatus,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            AtomSecurityGrade = reqObj.AtomSecurityGrade,
            AtomEmployeeRange = reqObj.AtomEmployeeRange
        };
        var coMgrCmpDbAddRspObj = await this._coMgrCompanyDb.AddAsync(coMgrCmpDbAddReqObj);
        if (coMgrCmpDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-關係公司-資料庫-新增多筆
        if (reqObj.ManagerCompanyAffiliateList != null && reqObj.ManagerCompanyAffiliateList.Count > 0)
        {
            var coMgrCmpAffDbAddManyReqObj = new CoMgrCmpAffDbAddManyReqMdl()
            {
                ManagerCompanyAffiliateList = reqObj.ManagerCompanyAffiliateList.Select(x => new CoMgrCmpAffDbAddManyReqItemMdl
                {
                    ManagerCompanyID = coMgrCmpDbAddRspObj.ManagerCompanyID,
                    ManagerCompanyAffiliateUnifiedNumber = x.ManagerCompanyAffiliateUnifiedNumber?.Trim(),
                    ManagerCompanyAffiliateName = x.ManagerCompanyAffiliateName?.Trim()
                }).ToList()
            };
            var coMgrCmpAffDbAddManyRspObj = await this._coMgrAffiliateCompanyDb.AddManyAsync(coMgrCmpAffDbAddManyReqObj);
            if (coMgrCmpAffDbAddManyRspObj == default)
            {
                // db, 核心-管理者-公司-資料庫-移除
                var coMgrCmpDbRemoveReqObj = new CoMgrCmpDbRemoveReqMdl()
                {
                    ManagerCompanyID = coMgrCmpDbAddRspObj.ManagerCompanyID
                };
                var coMgrCmpDbRemoveRspObj = await this._coMgrCompanyDb.RemoveAsync(coMgrCmpDbRemoveReqObj);
                if (coMgrCmpDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-管理者-公司營業地點-資料庫-新增多筆
        if (reqObj.ManagerCompanyLocationList != null && reqObj.ManagerCompanyLocationList.Count > 0)
        {
            var coMgrCmpLocDbAddManyReqObj = new CoMgrCmpLocDbAddManyReqMdl()
            {
                ManagerCompanyLocationList = reqObj.ManagerCompanyLocationList.Select(x => new CoMgrCmpLocDbAddManyReqItemMdl
                {
                    ManagerCompanyID = coMgrCmpDbAddRspObj.ManagerCompanyID,
                    ManagerCompanyLocationName = x.ManagerCompanyLocationName?.Trim(),
                    AtomCity = x.AtomCity,
                    ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress?.Trim(),
                    ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone?.Trim(),
                    ManagerCompanyLocationStatus = x.AtomManagerCompanyStatus
                }).ToList()
            };
            var coMgrCmpLocDbAddManyRspObj = await this._coMgrCompanyLocationDb.AddManyAsync(coMgrCmpLocDbAddManyReqObj);
            if (coMgrCmpLocDbAddManyRspObj == default)
            {
                // db, 核心-管理者-公司-資料庫-移除
                var coMgrCmpDbRemoveReqObj = new CoMgrCmpDbRemoveReqMdl()
                {
                    ManagerCompanyID = coMgrCmpDbAddRspObj.ManagerCompanyID
                };
                var coMgrCmpDbRemoveRspObj = await this._coMgrCompanyDb.RemoveAsync(coMgrCmpDbRemoveReqObj);
                if (coMgrCmpDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-管理者-關係公司-資料庫-依管理者公司ID移除
                var coMgrCmpAffDbRemoveByManagerCompanyIDReqObj = new CoMgrCmpAffDbRemoveByManagerCompanyIDReqMdl()
                {
                    ManagerCompanyID = coMgrCmpDbAddRspObj.ManagerCompanyID
                };
                var coMgrCmpAffDbRemoveByManagerCompanyIDRspObj = await this._coMgrAffiliateCompanyDb.RemoveByManagerCompanyIDAsync(coMgrCmpAffDbRemoveByManagerCompanyIDReqObj);
                if (coMgrCmpAffDbRemoveByManagerCompanyIDRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyID = coMgrCmpDbAddRspObj.ManagerCompanyID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-更新公司</summary>
    public async Task<MbsSysCmpLgcUpdateCompanyRspMdl> UpdateCompanyAsync(MbsSysCmpLgcUpdateCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcUpdateCompanyRspMdl
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

        if (reqObj.ManagerCompanyMainKindID.HasValue)
        {
            // db, 核心-管理者-公司主分類-資料庫-取得
            var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
            {
                ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID.Value
            };
            var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
            if (coMgrCmpMainKdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        if (reqObj.ManagerCompanySubKindID.HasValue)
        {
            // db, 核心-管理者-公司子分類-資料庫-取得
            var coMgrCmpSubKdDbGetReqObj = new CoMgrCmpSubKdDbGetReqMdl()
            {
                ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID.Value
            };
            var coMgrCmpSubKdDbGetRspObj = await this._coMgrCompanySubKindDb.GetAsync(coMgrCmpSubKdDbGetReqObj);
            if (coMgrCmpSubKdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 判斷統一編號是否需要更新
        if (string.IsNullOrWhiteSpace(reqObj.ManagerCompanyUnifiedNumber) == false)
        {
            // 統一編號需要更新，檢查統一編號是否重複

            // db, 核心-管理者-公司-資料庫-統一編號是否存在
            var coMgrComDbUniNumExistReqObj = new CoMgrComDbUniNumExistReqMdl()
            {
                UnifiedNumber = reqObj.ManagerCompanyUnifiedNumber,
            };
            var coMgrComDbUniNumExistRspObj = await this._coMgrCompanyDb.UniNumExistAsync(coMgrComDbUniNumExistReqObj);
            if (coMgrComDbUniNumExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrComDbUniNumExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 統一編號重複
                rspObj.ErrorCode = MbsErrorCodeEnum.UnifiedNumberDuplicate;
                return rspObj;
            }
        }

        // db, 核心-管理者-公司-資料庫-更新
        var coMgrCmpDbUpdateReqObj = new CoMgrCmpDbUpdateReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyUnifiedNumber = reqObj.ManagerCompanyUnifiedNumber?.Trim(),
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            AtomManagerCompanyStatus = reqObj.AtomManagerCompanyStatus,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            AtomSecurityGrade = reqObj.AtomSecurityGrade,
            AtomEmployeeRange = reqObj.AtomEmployeeRange
        };
        var coMgrCmpDbUpdateRspObj = await this._coMgrCompanyDb.UpdateAsync(coMgrCmpDbUpdateReqObj);
        if (coMgrCmpDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 公司營業地點

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司營業地點</summary>
    public async Task<MbsSysCmpLgcGetManyCompanyLocationRspMdl> GetManyCompanyLocationAsync(MbsSysCmpLgcGetManyCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetManyCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Success
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

        // db, 核心-管理者-公司營業地點-資料庫-取得多筆
        var coMgrCmpLocDbGetManyReqObj = new CoMgrCmpLocDbGetManyReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationIsDeleted = false,
        };
        var coMgrCmpLocDbGetManyRspObj = await this._coMgrCompanyLocationDb.GetManyAsync(coMgrCmpLocDbGetManyReqObj);
        if (coMgrCmpLocDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyLocationList = coMgrCmpLocDbGetManyRspObj.ManagerCompanyLocationList?
            .Select(x => new MbsSysCmpLgcGetManyCompanyLocationRspItemMdl()
            {
                ManagerCompanyLocationID = x.ManagerCompanyLocationID,
                ManagerCompanyLocationName = x.ManagerCompanyLocationName,
                AtomCity = x.AtomCity,
                ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress,
                ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone,
                AtomManagerCompanyStatus = x.ManagerCompanyLocationStatus
            }).ToList();
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司營業地點</summary>
    public async Task<MbsSysCmpLgcGetCompanyLocationRspMdl> GetCompanyLocationAsync(MbsSysCmpLgcGetCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetCompanyLocationRspMdl
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

        // db, 核心-管理者-公司營業地點-資料庫-取得
        var coMgrCmpLocDbGetReqObj = new CoMgrCmpLocDbGetReqMdl()
        {
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            ManagerCompanyLocationIsDeleted = false
        };
        var coMgrCmpLocDbGetRspObj = await this._coMgrCompanyLocationDb.GetAsync(coMgrCmpLocDbGetReqObj);
        if (coMgrCmpLocDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyLocationID = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationID;
        rspObj.ManagerCompanyLocationName = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationName;
        rspObj.AtomCity = coMgrCmpLocDbGetRspObj.AtomCity;
        rspObj.ManagerCompanyLocationAddress = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationAddress;
        rspObj.ManagerCompanyLocationTelephone = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationTelephone;
        rspObj.AtomManagerCompanyStatus = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationStatus;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司營業地點</summary>
    public async Task<MbsSysCmpLgcAddCompanyLocationRspMdl> AddCompanyLocationAsync(MbsSysCmpLgcAddCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcAddCompanyLocationRspMdl
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

        // db, 核心-管理者-公司-資料庫-是否存在
        var coMgrComDbExistsReqObj = new CoMgrComDbExistReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var coMgrComDbExistsRspObj = await this._coMgrCompanyDb.ExistAsync(coMgrComDbExistsReqObj);
        if (coMgrComDbExistsRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司營業地點-資料庫-新增
        var coMgrCmpLocDbAddReqObj = new CoMgrCmpLocDbAddReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationName = reqObj.ManagerCompanyLocationName?.Trim(),
            AtomCity = reqObj.AtomCity,
            ManagerCompanyLocationAddress = reqObj.ManagerCompanyLocationAddress?.Trim(),
            ManagerCompanyLocationTelephone = reqObj.ManagerCompanyLocationTelephone?.Trim(),
            ManagerCompanyLocationStatus = reqObj.AtomManagerCompanyStatus
        };
        var coMgrCmpLocDbAddRspObj = await this._coMgrCompanyLocationDb.AddAsync(coMgrCmpLocDbAddReqObj);
        if (coMgrCmpLocDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyLocationID = coMgrCmpLocDbAddRspObj.ManagerCompanyLocationID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯公司營業地點</summary>
    public async Task<MbsSysCmpLgcUpdateCompanyLocationRspMdl> UpdateCompanyLocationAsync(MbsSysCmpLgcUpdateCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcUpdateCompanyLocationRspMdl
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

        // db, 核心-管理者-公司營業地點-資料庫-更新
        var coMgrCmpLocDbUpdateReqObj = new CoMgrCmpLocDbUpdateReqMdl()
        {
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            ManagerCompanyLocationName = reqObj.ManagerCompanyLocationName,
            AtomCity = reqObj.AtomCity,
            ManagerCompanyLocationAddress = reqObj.ManagerCompanyLocationAddress,
            ManagerCompanyLocationTelephone = reqObj.ManagerCompanyLocationTelephone,
            ManagerCompanyLocationStatus = reqObj.AtomManagerCompanyStatus,
            ManagerCompanyLocationIsDeleted = reqObj.ManagerCompanyLocationIsDeleted
        };
        var coMgrCmpLocDbUpdateRspObj = await this._coMgrCompanyLocationDb.UpdateAsync(coMgrCmpLocDbUpdateReqObj);
        if (coMgrCmpLocDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 關係公司

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆關係公司</summary>
    public async Task<MbsSysCmpLgcGetManyCompanyAffiliateRspMdl> GetManyCompanyAffiliateAsync(MbsSysCmpLgcGetManyCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetManyCompanyAffiliateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Success
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

        // db, 核心-管理者-關係公司-資料庫-取得多筆
        var coMgrAffCmpDbGetManyReqObj = new CoMgrCmpAffDbGetManyReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyAffiliateIsDeleted = false,
        };
        var coMgrAffCmpDbGetManyRspObj = await this._coMgrAffiliateCompanyDb.GetManyAsync(coMgrAffCmpDbGetManyReqObj);
        if (coMgrAffCmpDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyAffiliateList = coMgrAffCmpDbGetManyRspObj.ManagerCompanyAffiliateList?
            .Select(x => new MbsSysCmpLgcGetManyCompanyAffiliateRspItemMdl()
            {
                ManagerCompanyAffiliateID = x.ManagerCompanyAffiliateID,
                ManagerCompanyAffiliateUnifiedNumber = x.ManagerCompanyAffiliateUnifiedNumber,
                ManagerCompanyAffiliateName = x.ManagerCompanyAffiliateName
            }).ToList();
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得關係公司</summary>
    public async Task<MbsSysCmpLgcGetCompanyAffiliateRspMdl> GetCompanyAffiliateAsync(MbsSysCmpLgcGetCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcGetCompanyAffiliateRspMdl
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

        // db, 核心-管理者-關係公司-資料庫-取得
        var coMgrAffCmpDbGetReqObj = new CoMgrCmpAffDbGetReqMdl()
        {
            ManagerCompanyAffiliateID = reqObj.ManagerCompanyAffiliateID,
            ManagerCompanyAffiliateIsDeleted = false,
        };
        var coMgrAffCmpDbGetRspObj = await this._coMgrAffiliateCompanyDb.GetAsync(coMgrAffCmpDbGetReqObj);
        if (coMgrAffCmpDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyAffiliateID = coMgrAffCmpDbGetRspObj.ManagerCompanyAffiliateID;
        rspObj.ManagerCompanyAffiliateUnifiedNumber = coMgrAffCmpDbGetRspObj.ManagerCompanyAffiliateUnifiedNumber?.Trim();
        rspObj.ManagerCompanyAffiliateName = coMgrAffCmpDbGetRspObj.ManagerCompanyAffiliateName;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增關係公司</summary>
    public async Task<MbsSysCmpLgcAddCompanyAffiliateRspMdl> AddCompanyAffiliateAsync(MbsSysCmpLgcAddCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcAddCompanyAffiliateRspMdl
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

        // db, 核心-管理者-公司-資料庫-是否存在
        var coMgrComDbExistsReqObj = new CoMgrComDbExistReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var coMgrComDbExistsRspObj = await this._coMgrCompanyDb.ExistAsync(coMgrComDbExistsReqObj);
        if (coMgrComDbExistsRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-關係公司-資料庫-新增
        var coMgrAffCmpDbAddReqObj = new CoMgrCmpAffDbAddReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyAffiliateUnifiedNumber = reqObj.ManagerCompanyAffiliateUnifiedNumber?.Trim(),
            ManagerCompanyAffiliateName = reqObj.ManagerCompanyAffiliateName?.Trim()
        };
        var coMgrAffCmpDbAddRspObj = await this._coMgrAffiliateCompanyDb.AddAsync(coMgrAffCmpDbAddReqObj);
        if (coMgrAffCmpDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerCompanyAffiliateID = coMgrAffCmpDbAddRspObj.ManagerCompanyAffiliateID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯關係公司</summary>
    public async Task<MbsSysCmpLgcUpdateCompanyAffiliateRspMdl> UpdateCompanyAffiliateAsync(MbsSysCmpLgcUpdateCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpLgcUpdateCompanyAffiliateRspMdl
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

        // db, 核心-管理者-關係公司-資料庫-更新
        var coMgrAffCmpDbUpdateReqObj = new CoMgrCmpAffDbUpdateReqMdl()
        {
            ManagerCompanyAffiliateID = reqObj.ManagerCompanyAffiliateID,
            ManagerCompanyAffiliateUnifiedNumber = reqObj.ManagerCompanyAffiliateUnifiedNumber?.Trim(),
            ManagerCompanyAffiliateName = reqObj.ManagerCompanyAffiliateName?.Trim(),
            ManagerCompanyAffiliateIsDeleted = reqObj.ManagerCompanyAffiliateIsDeleted
        };
        var coMgrAffCmpDbUpdateRspObj = await this._coMgrAffiliateCompanyDb.UpdateAsync(coMgrAffCmpDbUpdateReqObj);
        if (coMgrAffCmpDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion
}