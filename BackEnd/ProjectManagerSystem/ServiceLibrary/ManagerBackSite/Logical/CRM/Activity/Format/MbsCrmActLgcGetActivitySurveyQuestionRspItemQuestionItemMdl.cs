namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得活動問卷問題-回應項目項目模型</summary>
public class MbsCrmActLgcGetActivitySurveyQuestionRspItemQuestionItemMdl
{
    /// <summary>管理者活動問卷問題-問題項目ID</summary>
    public int ManagerActivitySurveyQuestionItemID { get; set; }

    /// <summary>管理者活動問卷問題-問題項目名稱</summary>
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者活動問卷問題-問題項目排序</summary>
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}