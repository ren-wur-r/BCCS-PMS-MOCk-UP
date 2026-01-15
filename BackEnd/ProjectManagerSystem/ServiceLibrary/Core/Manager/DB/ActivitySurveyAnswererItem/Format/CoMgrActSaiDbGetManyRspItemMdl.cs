namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Format;

/// <summary>核心-管理者-活動問卷回答項目-資料庫-取得多筆-回應項目模型</summary>
public class CoMgrActSaiDbGetManyRspItemMdl
{
    /// <summary>管理者活動問卷回答項目ID</summary>
    public int ManagerActivitySurveyAnswererItemID { get; set; }

    /// <summary>管理者活動問卷問題ID</summary>
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>管理者活動問卷問題項目ID</summary>
    public int ManagerActivitySurveyQuestionItemID { get; set; }

    /// <summary>是否勾選</summary>
    public bool IsChecked { get; set; }

    /// <summary>星等評分</summary>
    public short RatingValue { get; set; }

    /// <summary>回答內容</summary>
    public string Content { get; set; }

}