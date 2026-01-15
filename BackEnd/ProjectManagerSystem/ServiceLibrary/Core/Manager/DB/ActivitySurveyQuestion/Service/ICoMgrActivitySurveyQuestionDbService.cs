using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Service;

/// <summary>核心-管理者-活動問卷問題-資料庫服務</summary>
public interface ICoMgrActivitySurveyQuestionDbService
{
    /// <summary>核心-管理者-活動問卷問題-資料庫-新增</summary>
    Task<CoMgrAsqDbAddRspMdl> AddAsync(CoMgrAsqDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題-資料庫-新增多筆</summary>
    Task<CoMgrAsqDbAddManyRspMdl> AddManyAsync(CoMgrAsqDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題-資料庫-更新</summary>
    Task<CoMgrAsqDbUpdateRspMdl> UpdateAsync(CoMgrAsqDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題-資料庫-移除</summary>
    Task<CoMgrAsqDbRemoveRspMdl> RemoveAsync(CoMgrAsqDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題-資料庫-移除多筆</summary>
    Task<CoMgrAsqDbRemoveManyRspMdl> RemoveManyAsync(CoMgrAsqDbRemoveManyReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題-資料庫-取得</summary>
    Task<CoMgrAsqDbGetRspMdl> GetAsync(CoMgrAsqDbGetReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷問題-資料庫-取得多筆從[管理者活動ID]</summary>
    Task<CoMgrAsqDbGetManyFromActIdRspMdl> GetManyFromActIdAsync(CoMgrAsqDbGetManyFromActIdReqMdl reqObj);
}