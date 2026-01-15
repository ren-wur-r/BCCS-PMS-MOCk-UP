using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Service;

/// <summary>核心-管理者-活動問卷問題項目-資料庫服務</summary>
public interface ICoMgrActivitySurveyQuestionItemDbService
{
    /// <summary>核心-管理者-活動問卷問題項目-資料庫-新增</summary>
    public Task<CoMgrActSqiDbAddRspMdl> AddAsync(CoMgrActSqiDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-新增多筆</summary>
    public Task<CoMgrActSqiDbAddManyRspMdl> AddManyAsync(CoMgrActSqiDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-更新</summary>
    public Task<CoMgrActSqiDbUpdateRspMdl> UpdateAsync(CoMgrActSqiDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-移除</summary>
    public Task<CoMgrActSqiDbRemoveRspMdl> RemoveAsync(CoMgrActSqiDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-移除多筆</summary>
    public Task<CoMgrActSqiDbRemoveManyRspMdl> RemoveManyAsync(CoMgrActSqiDbRemoveManyReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-移除多筆從[管理者活動ID]</summary>
    public Task<CoMgrActSqiDbRemoveManyByActIdRspMdl> RemoveManyByActIdAsync(CoMgrActSqiDbRemoveManyByActIdReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-取得多筆從[管理者活動問卷問題ID]</summary>
    public Task<CoMgrActSqiDbGetManyFromAsqRspMdl> GetManyFromAsqAsync(CoMgrActSqiDbGetManyFromAsqReqMdl reqObj);
}