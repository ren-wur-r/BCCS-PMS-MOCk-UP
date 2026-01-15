using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Service;

/// <summary>核心-管理者-窗口開發評等原因-資料庫服務-介面</summary>
public interface ICoMgrContacterRatingReasonDbService
{
    /// <summary>核心-管理者-窗口開發評等原因-資料庫-是否存在</summary>
    public Task<CoMgrCttRtgRsnDbExistRspMdl> ExistAsync(CoMgrCttRtgRsnDbExistReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆</summary>
    public Task<CoMgrCttRtgRsnDbGetManyRspMdl> GetManyAsync(CoMgrCttRtgRsnDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得</summary>
    public Task<CoMgrCttRtgRsnDbGetRspMdl> GetAsync(CoMgrCttRtgRsnDbGetReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-新增</summary>
    public Task<CoMgrCttRtgRsnDbAddRspMdl> AddAsync(CoMgrCttRtgRsnDbAddReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-更新</summary>
    public Task<CoMgrCttRtgRsnDbUpdateRspMdl> UpdateAsync(CoMgrCttRtgRsnDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrCttRtgRsnDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCttRtgRsnDbGetManyNameReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-是否存在多筆</summary>
    public Task<CoMgrCttRtgRsnDbExistManyRspMdl> ExistManyAsync(CoMgrCttRtgRsnDbExistManyReqMdl reqObj);

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-窗口

    /// <summary>核心-管理者-窗口開發評等原因-資料庫服務-取得[筆數]從[管理者後台-系統設定-窗口]</summary>
    public Task<CoMgrCttRtgRsnDbGetCountFromMbsSysCtrRspMdl> GetCountFromMbsSysCtrAsync(CoMgrCttRtgRsnDbGetCountFromMbsSysCtrReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等原因-資料庫服務-取得多筆[管理者後台-系統設定-窗口]</summary>
    public Task<CoMgrCttRtgRsnDbGetManyFromMbsSysCtrRspMdl> GetManyFromMbsSysCtrAsync(CoMgrCttRtgRsnDbGetManyFromMbsSysCtrReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrCttRtgRsnDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCttRtgRsnDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}
