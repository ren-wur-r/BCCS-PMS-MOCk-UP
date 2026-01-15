using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Service;

/// <summary>管理者後台-系統設定-窗口-邏輯服務</summary>
public interface IMbsSysCtrLogicalService
{
    #region 窗口

    /// <summary>管理者後台-系統設定-窗口-取得多筆窗口</summary>
    public Task<MbsSysCtrLgcGetManyContacterRspMdl> GetManyContacterAsync(MbsSysCtrLgcGetManyContacterReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-取得單筆窗口</summary>
    public Task<MbsSysCtrLgcGetContacterRspMdl> GetContacterAsync(MbsSysCtrLgcGetContacterReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-新增窗口</summary>
    public Task<MbsSysCtrLgcAddContacterRspMdl> AddContacterAsync(MbsSysCtrLgcAddContacterReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-更新窗口</summary>
    public Task<MbsSysCtrLgcUpdateContacterRspMdl> UpdateContacterAsync(MbsSysCtrLgcUpdateContacterReqMdl reqObj);

    #endregion

    #region 窗口開發評等原因

    /// <summary>管理者後台-客戶名單-客戶窗口-取得多筆窗口開發評等原因</summary>
    public Task<MbsSysCtrLgcGetManyContacterRatingReasonRspMdl> GetManyContacterRatingReasonAsync(MbsSysCtrLgcGetManyContacterRatingReasonReqMdl reqObj);

    /// <summary>管理者後台-客戶名單-客戶窗口-取得單筆窗口開發評等原因</summary>
    public Task<MbsSysCtrLgcGetContacterRatingReasonRspMdl> GetContacterRatingReasonAsync(MbsSysCtrLgcGetContacterRatingReasonReqMdl reqObj);

    /// <summary>管理者後台-客戶名單-客戶窗口-新增窗口開發評等原因</summary>
    public Task<MbsSysCtrLgcAddContacterRatingReasonRspMdl> AddContacterRatingReasonAsync(MbsSysCtrLgcAddContacterRatingReasonReqMdl reqObj);

    /// <summary>管理者後台-客戶名單-客戶窗口-更新窗口開發評等原因</summary>
    public Task<MbsSysCtrLgcUpdateContacterRatingReasonRspMdl> UpdateContacterRatingReasonAsync(MbsSysCtrLgcUpdateContacterRatingReasonReqMdl reqObj);

    #endregion

    #region 窗口開發評等

    /// <summary>管理者後台-系統設定-窗口-取得多筆窗口開發評等</summary>
    public Task<MbsSysCtrLgcGetManyContacterRatingRspMdl> GetManyContacterRatingAsync(MbsSysCtrLgcGetManyContacterRatingReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-取得單筆窗口開發評等</summary>
    public Task<MbsSysCtrLgcGetContacterRatingRspMdl> GetContacterRatingAsync(MbsSysCtrLgcGetContacterRatingReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-新增窗口開發評等</summary>
    public Task<MbsSysCtrLgcAddContacterRatingRspMdl> AddContacterRatingAsync(MbsSysCtrLgcAddContacterRatingReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-更新窗口開發評等</summary>
    public Task<MbsSysCtrLgcUpdateContacterRatingRspMdl> UpdateContacterRatingAsync(MbsSysCtrLgcUpdateContacterRatingReqMdl reqObj);

    /// <summary>管理者後台-系統設定-窗口-移除窗口開發評等</summary>
    public Task<MbsSysCtrLgcRemoveContacterRatingRspMdl> RemoveContacterRatingAsync(MbsSysCtrLgcRemoveContacterRatingReqMdl reqObj);

    #endregion
}