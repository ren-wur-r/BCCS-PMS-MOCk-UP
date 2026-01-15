using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Service;

/// <summary>核心-管理者-活動問卷回答項目-資料庫服務</summary>
public interface ICoMgrActivitySurveyAnswererItemDbService
{
    /// <summary>核心-管理者-活動問卷回答項目-資料庫-取得多筆</summary>
    public Task<CoMgrActSaiDbGetManyRspMdl> GetManyAsync(CoMgrActSaiDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-活動問卷回答項目-資料庫-取得</summary>
    public Task<CoMgrActSaiDbGetRspMdl> GetAsync(CoMgrActSaiDbGetReqMdl reqObj);
}