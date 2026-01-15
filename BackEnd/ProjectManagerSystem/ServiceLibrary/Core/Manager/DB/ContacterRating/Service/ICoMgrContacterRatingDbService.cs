using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ContacterRating.Format;

namespace ServiceLibrary.Core.Manager.DB.ContacterRating.Service;

/// <summary>核心-管理者-窗口開發評等-資料庫服務</summary>
public interface ICoMgrContacterRatingDbService
{
    /// <summary>核心-管理者-窗口開發評等-資料庫服務-取得多筆</summary>
    public Task<CoMgrCttRtgDbGetManyRspMdl> GetManyAsync(CoMgrCttRtgDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-取得</summary> 
    public Task<CoMgrCttRtgDbGetRspMdl> GetAsync(CoMgrCttRtgDbGetReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-新增</summary>
    public Task<CoMgrCttRtgDbAddRspMdl> AddAsync(CoMgrCttRtgDbAddReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-新增多筆</summary>
    public Task<CoMgrCttRtgDbAddManyRspMdl> AddManyAsync(CoMgrCttRtgDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-更新</summary>
    public Task<CoMgrCttRtgDbUpdateRspMdl> UpdateAsync(CoMgrCttRtgDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-移除</summary>
    public Task<CoMgrCttRtgDbRemoveRspMdl> RemoveAsync(CoMgrCttRtgDbRemoveReqMdl reqObj);
}
