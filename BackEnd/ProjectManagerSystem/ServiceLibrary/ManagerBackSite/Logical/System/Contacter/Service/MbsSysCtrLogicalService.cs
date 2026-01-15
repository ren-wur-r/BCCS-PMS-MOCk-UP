using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.AtomManagerContacter;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Company.Format;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.Core.Manager.DB.Contacter.Format;
using ServiceLibrary.Core.Manager.DB.Contacter.Service;
using ServiceLibrary.Core.Manager.DB.ContacterRating.Format;
using ServiceLibrary.Core.Manager.DB.ContacterRating.Service;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Service;

/// <summary>管理者後台-系統設定-窗口-邏輯服務</summary>
public class MbsSysCtrLogicalService : IMbsSysCtrLogicalService
{
    /// <summary>Logger</summary>
    private readonly ILogger<MbsSysCtrLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-公司-資料庫服務</summary>
    private readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    /// <summary>核心-管理者-窗口-資料庫服務</summary>
    private readonly ICoMgrContacterDbService _coMgrContacterDb;

    /// <summary>核心-管理者-窗口開發評等-資料庫服務</summary>
    private readonly ICoMgrContacterRatingDbService _coMgrContacterRatingDb;

    /// <summary>核心-管理者-窗口開發評等原因-資料庫服務</summary>
    private readonly ICoMgrContacterRatingReasonDbService _coMgrContacterRatingReasonDb;

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
    public MbsSysCtrLogicalService(
        ILogger<MbsSysCtrLogicalService> logger,
        // Core Manager
        ICoMgrCompanyDbService coMgrCompanyDb,
        ICoMgrContacterDbService coMgrContacterDb,
        ICoMgrContacterRatingDbService coMgrContacterRatingDb,
        ICoMgrContacterRatingReasonDbService coMgrContacterRatingReasonDb,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrCompanyDb = coMgrCompanyDb;
        this._coMgrContacterDb = coMgrContacterDb;
        this._coMgrContacterRatingDb = coMgrContacterRatingDb;
        this._coMgrContacterRatingReasonDb = coMgrContacterRatingReasonDb;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
        // This
        this._AtomMenu = DbAtomMenuEnum.SystemContacter;
    }

    #region 窗口

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-取得多筆窗口</summary>
    public async Task<MbsSysCtrLgcGetManyContacterRspMdl> GetManyContacterAsync(MbsSysCtrLgcGetManyContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcGetManyContacterRspMdl
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

        // db, 核心-管理者-窗口-資料庫-取得[筆數]從[管理者後台-系統設定-窗口]
        var coMgrCttDbGetCountFrommbsSysCtrReqObj = new CoMgrCttDbGetCountFromMbsSysCtrReqMdl()
        {
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
        };
        var coMgrCttDbGetCountFrommbsSysCtrRspObj = await this._coMgrContacterDb.GetCountFromMbsSysCtrAsync(coMgrCttDbGetCountFrommbsSysCtrReqObj);
        if (coMgrCttDbGetCountFrommbsSysCtrRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrCttDbGetCountFrommbsSysCtrRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerContacterList = new List<MbsSysCtrLgcGetManyContacterRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫-取得多筆從[管理者後台-系統設定-窗口]
        var coMgrCttDbGetManyFrommbsSysCtrReqObj = new CoMgrCttDbGetManyFromMbsSysCtrReqMdl()
        {
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrCttDbGetManyFrommbsSysCtrRspObj = await this._coMgrContacterDb.GetManyFromMbsSysCtrAsync(coMgrCttDbGetManyFrommbsSysCtrReqObj);
        if (coMgrCttDbGetManyFrommbsSysCtrRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得多筆[名稱]
        var coMgrCmpDbGetManyNameReqObj = new CoMgrCmpDbGetManyNameReqMdl()
        {
            ManagerCompanyIdList = coMgrCttDbGetManyFrommbsSysCtrRspObj.ManagerContacterList
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
        rspObj.ManagerContacterList =
        (
            from x in coMgrCttDbGetManyFrommbsSysCtrRspObj.ManagerContacterList
            from y in coMgrCmpDbGetManyNameRspObj.ManagerCompanyList
                            .Where(y => y.ManagerCompanyID == x.ManagerCompanyID)
                            .DefaultIfEmpty()
            select new MbsSysCtrLgcGetManyContacterRspItemMdl
            {
                ManagerContacterID = x.ManagerContacterID,
                ManagerCompanyName = y?.ManagerCompanyName,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail,
                ManagerContacterPhone = x.ManagerContacterPhone,
                ManagerContacterDepartment = x.ManagerContacterDepartment,
                ManagerContacterJobTitle = x.ManagerContacterJobTitle,
                ManagerContacterRatingKind = x.ManagerContacterRatingKind
            }
        ).ToList();
        rspObj.TotalCount = coMgrCttDbGetCountFrommbsSysCtrRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-取得單筆窗口</summary>
    public async Task<MbsSysCtrLgcGetContacterRspMdl> GetContacterAsync(MbsSysCtrLgcGetContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcGetContacterRspMdl
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

        // db, 核心-管理者-窗口-資料庫-取得
        var coMgrCttDbGetReqObj = new CoMgrCttDbGetReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID,
        };
        var coMgrCttDbGetRspObj = await this._coMgrContacterDb.GetAsync(coMgrCttDbGetReqObj);
        if (coMgrCttDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得[名稱]
        var coMgrCmpDbGetNameReqObj = new CoMgrCmpDbGetNameReqMdl()
        {
            ManagerCompanyID = coMgrCttDbGetRspObj.ManagerCompanyID
        };
        var coMgrCmpDbGetNameRspObj = await this._coMgrCompanyDb.GetNameAsync(coMgrCmpDbGetNameReqObj);
        if (coMgrCmpDbGetNameRspObj == default)
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
        rspObj.ManagerContacterRatingKind = coMgrCttDbGetRspObj.AtomRatingKind;
        rspObj.ManagerContacterCreateEmployeeID = coMgrCttDbGetRspObj.ManagerContacterCreateEmployeeID;
        rspObj.ManagerContacterRemark = coMgrCttDbGetRspObj.ManagerContacterRemark;
        rspObj.ManagerCompanyName = coMgrCmpDbGetNameRspObj.ManagerCompanyName;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-新增窗口</summary>
    public async Task<MbsSysCtrLgcAddContacterRspMdl> AddContacterAsync(MbsSysCtrLgcAddContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcAddContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口-資料庫-檢查同公司Email是否存在
        var coMgrCttDbExistByCompanyEmailReqObj = new CoMgrCttDbExistByCompanyEmailReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterEmail = reqObj.ManagerContacterEmail
        };
        var coMgrCttDbExistByCompanyEmailRspObj = await this._coMgrContacterDb.ExistByCompanyEmailAsync(coMgrCttDbExistByCompanyEmailReqObj);
        if (coMgrCttDbExistByCompanyEmailRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        if (coMgrCttDbExistByCompanyEmailRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 窗口Email重複
            rspObj.ErrorCode = MbsErrorCodeEnum.ContacterEmailDuplicate;
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-是否存在
        var coMgrComDbExistReqObj = new CoMgrComDbExistReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var coMgrComDbExistRspObj = await this._coMgrCompanyDb.ExistAsync(coMgrComDbExistReqObj);
        if (coMgrComDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        if (reqObj.ManagerContacterRatingList != null && reqObj.ManagerContacterRatingList.Count > 0)
        {
            // db, 核心-管理者-窗口開發評等原因-資料庫-是否存在多筆
            var coMgrCttRtgRsnDbExistManyReqObj = new CoMgrCttRtgRsnDbExistManyReqMdl()
            {
                ManagerContacterRatingReasonIDList = reqObj.ManagerContacterRatingList
                                                            .Select(x => x.ManagerContacterRatingReasonID)
                                                            .Distinct()
                                                            .ToList()
            };
            var coMgrCttRtgRsnDbExistManyRspObj = await this._coMgrContacterRatingReasonDb.ExistManyAsync(coMgrCttRtgRsnDbExistManyReqObj);
            if (coMgrCttRtgRsnDbExistManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (!coMgrCttRtgRsnDbExistManyRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-管理者-窗口-資料庫-新增
        var coMgrCttDbAddReqObj = new CoMgrCttDbAddReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            ManagerContacterPhone = reqObj.ManagerContacterPhone,
            ManagerContacterDepartment = reqObj.ManagerContacterDepartment,
            ManagerContacterJobTitle = reqObj.ManagerContacterJobTitle,
            ManagerContacterTelephone = reqObj.ManagerContacterTelephone,
            ManagerContacterStatus = reqObj.ManagerContacterStatus,
            ManagerContacterIsConsent = reqObj.ManagerContacterIsConsent,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
            ManagerContacterCreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            ManagerContacterRemark = reqObj.ManagerContacterRemark?.Trim()
        };
        var coMgrCttDbAddRspObj = await this._coMgrContacterDb.AddAsync(coMgrCttDbAddReqObj);
        if (coMgrCttDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口開發評等-資料庫服務-新增多筆
        if (reqObj.ManagerContacterRatingList != null && reqObj.ManagerContacterRatingList.Count > 0)
        {
            var coMgrCttRtgDbAddManyReqObj = new CoMgrCttRtgDbAddManyReqMdl()
            {
                ManagerContacterRatingList = reqObj.ManagerContacterRatingList.Select(x => new CoMgrCttRtgDbAddManyReqItemMdl
                {
                    ManagerContacterID = coMgrCttDbAddRspObj.ManagerContacterID,
                    ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                    ManagerContacterRatingRemark = x.ManagerContacterRatingRemark
                }).ToList()
            };
            var coMgrCttRtgDbAddManyRspObj = await this._coMgrContacterRatingDb.AddManyAsync(coMgrCttRtgDbAddManyReqObj);
            if (coMgrCttRtgDbAddManyRspObj == default)
            {
                // db, 核心-管理者-窗口-資料庫服務-移除
                var coMgrCttDbRemoveReqObj = new CoMgrCttDbRemoveReqMdl()
                {
                    ManagerContacterID = coMgrCttDbAddRspObj.ManagerContacterID
                };
                var coMgrCttDbRemoveRspObj = await this._coMgrContacterDb.RemoveAsync(coMgrCttDbRemoveReqObj);
                if (coMgrCttDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterID = coMgrCttDbAddRspObj.ManagerContacterID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-更新窗口</summary>
    public async Task<MbsSysCtrLgcUpdateContacterRspMdl> UpdateContacterAsync(MbsSysCtrLgcUpdateContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcUpdateContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口-資料庫-取得
        var coMgrCttDbGetReqObj = new CoMgrCttDbGetReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID,
        };
        var coMgrCttDbGetRspObj = await this._coMgrContacterDb.GetAsync(coMgrCttDbGetReqObj);
        if (coMgrCttDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷是否修改公司ID
        if (reqObj.ManagerCompanyID.HasValue &&
            reqObj.ManagerCompanyID.Value != coMgrCttDbGetRspObj.ManagerCompanyID)
        {
            //若是有，舊的改離職 + 新增新窗口

            // db, 核心-管理者-窗口-資料庫-檢查同公司Email是否存在
            var coMgrCttDbExistByCompanyEmailReqObj = new CoMgrCttDbExistByCompanyEmailReqMdl()
            {
                ManagerCompanyID = reqObj.ManagerCompanyID.Value,
                ManagerContacterEmail = reqObj.ManagerContacterEmail ?? coMgrCttDbGetRspObj.ManagerContacterEmail
            };
            var coMgrCttDbExistByCompanyEmailRspObj = await this._coMgrContacterDb.ExistByCompanyEmailAsync(coMgrCttDbExistByCompanyEmailReqObj);
            if (coMgrCttDbExistByCompanyEmailRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            if (coMgrCttDbExistByCompanyEmailRspObj.IsExist)
            {
                this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.ContacterEmailDuplicate;
                return rspObj;
            }

            // db, 核心-管理者-窗口-資料庫-更新窗口
            var coMgrCttDbUpdateOldReqObj = new CoMgrCttDbUpdateReqMdl()
            {
                ManagerContacterID = reqObj.ManagerContacterID,
                ManagerContacterStatus = DbAtomManagerContacterStatusEnum.Unemployed
            };
            var coMgrCttDbUpdateOldRspObj = await this._coMgrContacterDb.UpdateAsync(coMgrCttDbUpdateOldReqObj);
            if (coMgrCttDbUpdateOldRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-管理者-窗口-資料庫-新增窗口
            var coMgrCttDbAddReqObj = new CoMgrCttDbAddReqMdl()
            {
                ManagerCompanyID = reqObj.ManagerCompanyID.Value,
                ManagerContacterName = reqObj.ManagerContacterName ?? coMgrCttDbGetRspObj.ManagerContacterName,
                ManagerContacterEmail = reqObj.ManagerContacterEmail ?? coMgrCttDbGetRspObj.ManagerContacterEmail,
                ManagerContacterPhone = reqObj.ManagerContacterPhone ?? coMgrCttDbGetRspObj.ManagerContacterPhone,
                ManagerContacterDepartment = reqObj.ManagerContacterDepartment ?? coMgrCttDbGetRspObj.ManagerContacterDepartment,
                ManagerContacterJobTitle = reqObj.ManagerContacterJobTitle ?? coMgrCttDbGetRspObj.ManagerContacterJobTitle,
                ManagerContacterTelephone = reqObj.ManagerContacterTelephone ?? coMgrCttDbGetRspObj.ManagerContacterTelephone,
                ManagerContacterStatus = reqObj.ManagerContacterStatus ?? coMgrCttDbGetRspObj.ManagerContacterStatus,
                ManagerContacterIsConsent = reqObj.ManagerContacterIsConsent ?? coMgrCttDbGetRspObj.ManagerContacterIsConsent,
                ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind ?? coMgrCttDbGetRspObj.AtomRatingKind,
                ManagerContacterCreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
                ManagerContacterRemark = reqObj.ManagerContacterRemark?.Trim() ?? coMgrCttDbGetRspObj.ManagerContacterRemark
            };
            var coMgrCttDbAddRspObj = await this._coMgrContacterDb.AddAsync(coMgrCttDbAddReqObj);
            if (coMgrCttDbAddRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerContacterID = coMgrCttDbAddRspObj.ManagerContacterID;
            return rspObj;
        }


        // db, 核心-管理者-窗口-資料庫-更新
        var coMgrCttDbUpdateReqObj = new CoMgrCttDbUpdateReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID,
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            ManagerContacterPhone = reqObj.ManagerContacterPhone,
            ManagerContacterDepartment = reqObj.ManagerContacterDepartment,
            ManagerContacterJobTitle = reqObj.ManagerContacterJobTitle,
            ManagerContacterTelephone = reqObj.ManagerContacterTelephone,
            ManagerContacterStatus = reqObj.ManagerContacterStatus,
            ManagerContacterIsConsent = reqObj.ManagerContacterIsConsent,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
            ManagerContacterRemark = reqObj.ManagerContacterRemark?.Trim()
        };
        var coMgrCttDbUpdateRspObj = await this._coMgrContacterDb.UpdateAsync(coMgrCttDbUpdateReqObj);
        if (coMgrCttDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 窗口開發評等原因

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-取得多筆窗口開發評等原因</summary>
    public async Task<MbsSysCtrLgcGetManyContacterRatingReasonRspMdl> GetManyContacterRatingReasonAsync(MbsSysCtrLgcGetManyContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcGetManyContacterRatingReasonRspMdl
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

        // db, 核心-管理者-窗口開發評等原因-資料庫-取得[筆數]從[管理者後台-系統設定-窗口]
        var coMgrCttRtgRsnDbGetCountFrommbsSysCtrReqObj = new CoMgrCttRtgRsnDbGetCountFromMbsSysCtrReqMdl()
        {
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName,
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
        };
        var coMgrCttRtgRsnDbGetCountFrommbsSysCtrRspObj = await this._coMgrContacterRatingReasonDb.GetCountFromMbsSysCtrAsync(coMgrCttRtgRsnDbGetCountFrommbsSysCtrReqObj);
        if (coMgrCttRtgRsnDbGetCountFrommbsSysCtrRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrCttRtgRsnDbGetCountFrommbsSysCtrRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerContacterRatingReasonList = new List<MbsSysCtrLgcGetManyContacterRatingReasonRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-窗口開發評等原因-資料庫-取得[多筆]從[管理者後台-系統設定-窗口]
        var coMgrCttRtgRsnDbGetManyFrommbsSysCtrReqObj = new CoMgrCttRtgRsnDbGetManyFromMbsSysCtrReqMdl()
        {
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName,
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrCttRtgRsnDbGetManyFrommbsSysCtrRspObj = await this._coMgrContacterRatingReasonDb.GetManyFromMbsSysCtrAsync(coMgrCttRtgRsnDbGetManyFrommbsSysCtrReqObj);
        if (coMgrCttRtgRsnDbGetManyFrommbsSysCtrRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingReasonList = coMgrCttRtgRsnDbGetManyFrommbsSysCtrRspObj.ManagerContacterRatingReasonList
            .Select(x => new MbsSysCtrLgcGetManyContacterRatingReasonRspItemMdl
            {
                ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                ManagerContacterRatingReasonName = x.ManagerContacterRatingReasonName,
                ManagerContacterRatingReasonStatus = x.ManagerContacterRatingReasonStatus
            }).ToList();
        rspObj.TotalCount = coMgrCttRtgRsnDbGetCountFrommbsSysCtrRspObj.Count;

        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-取得單筆窗口開發評等原因</summary>
    public async Task<MbsSysCtrLgcGetContacterRatingReasonRspMdl> GetContacterRatingReasonAsync(MbsSysCtrLgcGetContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcGetContacterRatingReasonRspMdl
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

        // db, 核心-管理者-窗口開發評等原因-資料庫-取得
        var coMgrCttRtgRsnDbGetReqObj = new CoMgrCttRtgRsnDbGetReqMdl()
        {
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
        };
        var coMgrCttRtgRsnDbGetRspObj = await this._coMgrContacterRatingReasonDb.GetAsync(coMgrCttRtgRsnDbGetReqObj);
        if (coMgrCttRtgRsnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingReasonName = coMgrCttRtgRsnDbGetRspObj.ManagerContacterRatingReasonName;
        rspObj.ManagerContacterRatingReasonStatus = coMgrCttRtgRsnDbGetRspObj.ManagerContacterRatingReasonStatus;

        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-新增窗口開發評等原因</summary>
    public async Task<MbsSysCtrLgcAddContacterRatingReasonRspMdl> AddContacterRatingReasonAsync(MbsSysCtrLgcAddContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcAddContacterRatingReasonRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口開發評等原因-資料庫-新增
        var coMgrCttRtgRsnDbAddReqObj = new CoMgrCttRtgRsnDbAddReqMdl()
        {
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName,
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
        };
        var coMgrCttRtgRsnDbAddRspObj = await this._coMgrContacterRatingReasonDb.AddAsync(coMgrCttRtgRsnDbAddReqObj);
        if (coMgrCttRtgRsnDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingReasonID = coMgrCttRtgRsnDbAddRspObj.ManagerContacterRatingReasonID;

        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-更新窗口開發評等原因</summary>
    public async Task<MbsSysCtrLgcUpdateContacterRatingReasonRspMdl> UpdateContacterRatingReasonAsync(MbsSysCtrLgcUpdateContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcUpdateContacterRatingReasonRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口開發評等原因-資料庫-更新
        var coMgrCttRtgRsnDbUpdateReqObj = new CoMgrCttRtgRsnDbUpdateReqMdl()
        {
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName,
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
        };
        var coMgrCttRtgRsnDbUpdateRspObj = await this._coMgrContacterRatingReasonDb.UpdateAsync(coMgrCttRtgRsnDbUpdateReqObj);
        if (coMgrCttRtgRsnDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;

        return rspObj;
    }

    #endregion

    #region 窗口開發評等

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-取得多筆窗口開發評等</summary>
    public async Task<MbsSysCtrLgcGetManyContacterRatingRspMdl> GetManyContacterRatingAsync(MbsSysCtrLgcGetManyContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcGetManyContacterRatingRspMdl
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

        // db, 核心-管理者-窗口開發評等-資料庫-取得多筆
        var coMgrCttRtgDbGetManyReqObj = new CoMgrCttRtgDbGetManyReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID,
        };
        var coMgrCttRtgDbGetManyRspObj = await this._coMgrContacterRatingDb.GetManyAsync(coMgrCttRtgDbGetManyReqObj);
        if (coMgrCttRtgDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口開發評等原因-資料庫-取得多筆[名稱]
        var coMgrCttRtgRsnDbGetManyNameReqObj = new CoMgrCttRtgRsnDbGetManyNameReqMdl()
        {
            ManagerContacterRatingReasonIDList = coMgrCttRtgDbGetManyRspObj.ManagerContacterRatingList
                                                    .Select(x => x.ManagerContacterRatingReasonID)
                                                    .Distinct()
                                                    .ToList()
        };
        var coMgrCttRtgRsnDbGetManyNameRspObj = await this._coMgrContacterRatingReasonDb.GetManyNameAsync(coMgrCttRtgRsnDbGetManyNameReqObj);
        if (coMgrCttRtgRsnDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingList =
        (
            from x in coMgrCttRtgDbGetManyRspObj.ManagerContacterRatingList
            from y in coMgrCttRtgRsnDbGetManyNameRspObj.ManagerContacterRatingReasonList
                            .Where(y => y.ManagerContacterRatingReasonID == x.ManagerContacterRatingReasonID)
                            .DefaultIfEmpty()
            select new MbsSysCtrLgcGetManyContacterRatingRspItemMdl
            {
                ManagerContacterRatingID = x.ManagerContacterRatingID,
                ManagerContacterRatingRemark = x.ManagerContacterRatingRemark,
                ManagerContacterRatingReasonName = y?.ManagerContacterRatingReasonName
            }
        )
        .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-取得單筆窗口開發評等</summary>
    public async Task<MbsSysCtrLgcGetContacterRatingRspMdl> GetContacterRatingAsync(MbsSysCtrLgcGetContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcGetContacterRatingRspMdl
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

        // db, 核心-管理者-窗口開發評等-資料庫-取得
        var coMgrCttRtgDbGetReqObj = new CoMgrCttRtgDbGetReqMdl()
        {
            ManagerContacterRatingID = reqObj.ManagerContacterRatingID,
        };
        var coMgrCttRtgDbGetRspObj = await this._coMgrContacterRatingDb.GetAsync(coMgrCttRtgDbGetReqObj);
        if (coMgrCttRtgDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingReasonID = coMgrCttRtgDbGetRspObj.ManagerContacterRatingReasonID;
        rspObj.ManagerContacterRatingRemark = coMgrCttRtgDbGetRspObj.ManagerContacterRatingRemark;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-新增窗口開發評等</summary>
    public async Task<MbsSysCtrLgcAddContacterRatingRspMdl> AddContacterRatingAsync(MbsSysCtrLgcAddContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcAddContacterRatingRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口-資料庫服務-是否存在
        var coMgrCttDbExistReqObj = new CoMgrCttDbExistReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID
        };
        var coMgrCttDbExistRspObj = await this._coMgrContacterDb.ExistAsync(coMgrCttDbExistReqObj);
        if (coMgrCttDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口開發評等原因-資料庫-是否存在
        var coMgrCttRtgRsnDbExistReqObj = new CoMgrCttRtgRsnDbExistReqMdl()
        {
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID
        };
        var coMgrCttRtgRsnDbExistRspObj = await this._coMgrContacterRatingReasonDb.ExistAsync(coMgrCttRtgRsnDbExistReqObj);
        if (coMgrCttRtgRsnDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口開發評等-資料庫-新增
        var coMgrCttRtgDbAddReqObj = new CoMgrCttRtgDbAddReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID,
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
            ManagerContacterRatingRemark = reqObj.ManagerContacterRatingRemark,
        };
        var coMgrCttRtgDbAddRspObj = await this._coMgrContacterRatingDb.AddAsync(coMgrCttRtgDbAddReqObj);
        if (coMgrCttRtgDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerContacterRatingID = coMgrCttRtgDbAddRspObj.ManagerContacterRatingID;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-更新窗口開發評等</summary>
    public async Task<MbsSysCtrLgcUpdateContacterRatingRspMdl> UpdateContacterRatingAsync(MbsSysCtrLgcUpdateContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcUpdateContacterRatingRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        if (reqObj.ManagerContacterRatingReasonID.HasValue)
        {
            // db, 核心-管理者-窗口開發評等原因-資料庫-是否存在
            var coMgrCttRtgRsnDbExistReqObj = new CoMgrCttRtgRsnDbExistReqMdl()
            {
                ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID.Value
            };
            var coMgrCttRtgRsnDbExistRspObj = await this._coMgrContacterRatingReasonDb.ExistAsync(coMgrCttRtgRsnDbExistReqObj);
            if (coMgrCttRtgRsnDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-管理者-窗口開發評等-資料庫-更新
        var coMgrCttRtgDbUpdateReqObj = new CoMgrCttRtgDbUpdateReqMdl()
        {
            ManagerContacterRatingID = reqObj.ManagerContacterRatingID,
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
            ManagerContacterRatingRemark = reqObj.ManagerContacterRatingRemark,
        };
        var coMgrCttRtgDbUpdateRspObj = await this._coMgrContacterRatingDb.UpdateAsync(coMgrCttRtgDbUpdateReqObj);
        if (coMgrCttRtgDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-系統設定-窗口-邏輯服務-移除窗口開發評等</summary>
    public async Task<MbsSysCtrLgcRemoveContacterRatingRspMdl> RemoveContacterRatingAsync(MbsSysCtrLgcRemoveContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrLgcRemoveContacterRatingRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口開發評等-資料庫-移除
        var coMgrCttRtgDbRemoveReqObj = new CoMgrCttRtgDbRemoveReqMdl()
        {
            ManagerContacterRatingID = reqObj.ManagerContacterRatingID,
        };
        var coMgrCttRtgDbRemoveRspObj = await this._coMgrContacterRatingDb.RemoveAsync(coMgrCttRtgDbRemoveReqObj);
        if (coMgrCttRtgDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion
}