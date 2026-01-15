using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Service;

/// <summary>核心-管理者-活動問卷回答者-資料庫服務</summary>
public interface ICoMgrActivitySurveyAnswererDbService
{
    /// <summary>核心-管理者-活動問卷回答者-資料庫-新增</summary>
    public Task<CoMgrAsaDbAddRspMdl> AddAsync(CoMgrAsaDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷回答者-資料庫-更新</summary>
    public Task<CoMgrAsaDbUpdateRspMdl> UpdateAsync(CoMgrAsaDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷回答者-資料庫-移除</summary>
    public Task<CoMgrAsaDbRemoveRspMdl> RemoveAsync(CoMgrAsaDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷回答者-資料庫-取得</summary>
    public Task<CoMgrAsaDbGetRspMdl> GetAsync(CoMgrAsaDbGetReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷回答者-資料庫-取得筆數從[管理者活動ID]</summary>
    public Task<CoMgrAsaDbGetCountFromActivityIdRspMdl> GetCountFromActivityIdAsync(CoMgrAsaDbGetCountFromActivityIdReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷回答者-資料庫-取得多筆從[管理者活動ID]</summary>
    public Task<CoMgrAsaDbGetManyFromActivityIdRspMdl> GetManyFromActivityIdAsync(CoMgrAsaDbGetManyFromActivityIdReqMdl reqObj);

}