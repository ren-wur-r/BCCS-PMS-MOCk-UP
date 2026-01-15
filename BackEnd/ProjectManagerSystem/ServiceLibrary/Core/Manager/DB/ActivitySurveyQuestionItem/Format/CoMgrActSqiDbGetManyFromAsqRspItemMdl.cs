namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;

/// <summary>核心-管理者-活動問卷問題項目-資料庫-取得多筆從[管理者活動問卷問題ID]-回應項目模型</summary>
public class CoMgrActSqiDbGetManyFromAsqRspItemMdl
{
    /// <summary>管理者活動問卷問題項目-ID</summary>
    public int ManagerActivitySurveyQuestionItemID { get; set; }

    /// <summary>管理者活動問卷問題項目-名稱</summary>
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者活動問卷問題項目-排序</summary>
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}