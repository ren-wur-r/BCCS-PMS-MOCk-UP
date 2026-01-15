using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

/// <summary>核心-管理者-活動問卷問題-資料庫-取得多筆從[管理者活動ID]-回應模型</summary>
public class CoMgrAsqDbGetManyFromActIdRspMdl
{
    /// <summary>管理者活動問卷問題清單</summary>
    public List<CoMgrAsqDbGetManyFromActIdRspItemMdl> ManagerActivitySurveyQuestionList { get; set; }
}
