using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Service;

/// <summary>核心-管理者-活動贊助商-資料庫服務介面</summary>
public interface ICoMgrActivitySponsorDbService
{
    /// <summary>核心-管理者-活動贊助商-資料庫-是否存在</summary>
    public Task<CoMgrActSpsDbExistRspMdl> ExistAsync(CoMgrActSpsDbExistReqMdl reqObj);

    /// <summary>核心-管理者-活動贊助商-資料庫-新增</summary>
    public Task<CoMgrActSpsDbAddRspMdl> AddAsync(CoMgrActSpsDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動贊助商-資料庫-新增多筆</summary>
    public Task<CoMgrActSpsDbAddManyRspMdl> AddManyAsync(CoMgrActSpsDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-活動贊助商-資料庫-更新</summary>
    public Task<CoMgrActSpsDbUpdateRspMdl> UpdateAsync(CoMgrActSpsDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動贊助商-資料庫-移除</summary>
    public Task<CoMgrActSpsDbRemoveRspMdl> RemoveAsync(CoMgrActSpsDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動贊助商-資料庫-取得</summary>
    public Task<CoMgrActSpsDbGetRspMdl> GetAsync(CoMgrActSpsDbGetReqMdl reqObj);

    /// <summary>核心-管理者-活動贊助商-資料庫-取得多筆</summary>
    public Task<CoMgrActSpsDbGetManyRspMdl> GetManyAsync(CoMgrActSpsDbGetManyReqMdl reqObj);

    #region ManagerBackSite.Marketing.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-活動贊助商-資料庫-取得多筆總贊助金額從[管理者後台-CRM-活動管理]</summary>
    public Task<CoMgrActSpsDbGetManyTotalSponsorAmountRspMdl> GetManyTotalSponsorAmountFromMbsMktActAsync(CoMgrActSpsDbGetManyTotalSponsorAmountReqMdl reqObj);

    #endregion
}
