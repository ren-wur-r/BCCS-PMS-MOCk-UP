namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Format;

/// <summary>核心-管理者-活動問卷回答項目-資料庫-取得-回應模型</summary>
public class CoMgrActSaiDbGetRspMdl
{
    /// <summary>管理者活動問卷回答者ID</summary>
    public int ManagerActivitySurveyAnswererID { get; set; }

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