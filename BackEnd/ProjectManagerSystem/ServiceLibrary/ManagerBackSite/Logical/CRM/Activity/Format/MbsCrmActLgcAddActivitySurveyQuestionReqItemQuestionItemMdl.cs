namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動問卷問題-選項項目請求模型</summary>
public class MbsCrmActLgcAddActivitySurveyQuestionReqItemQuestionItemMdl
{
    /// <summary>管理者活動問卷問題項目-名稱</summary>
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者活動問卷問題項目-排序</summary>
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}