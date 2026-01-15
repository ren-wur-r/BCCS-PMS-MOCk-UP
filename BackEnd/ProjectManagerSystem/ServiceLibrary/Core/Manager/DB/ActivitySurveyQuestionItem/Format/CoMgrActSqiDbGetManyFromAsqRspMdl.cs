using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;

/// <summary>核心-管理者-活動問卷問題項目-資料庫-取得多筆從[管理者活動問卷問題ID]-回應模型</summary>
public class CoMgrActSqiDbGetManyFromAsqRspMdl
{
    /// <summary>管理者活動問卷問題項目清單</summary>
    public List<CoMgrActSqiDbGetManyFromAsqRspItemMdl> ManagerActivitySurveyQuestionItemList { get; set; }
}