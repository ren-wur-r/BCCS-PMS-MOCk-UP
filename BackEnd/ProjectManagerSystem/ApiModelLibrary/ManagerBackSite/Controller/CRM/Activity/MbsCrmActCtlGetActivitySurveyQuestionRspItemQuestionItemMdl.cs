using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得活動問卷問題-回應項目項目模型</summary>
public class MbsCrmActCtlGetActivitySurveyQuestionRspItemQuestionItemMdl
{
    /// <summary>管理者活動問卷問題-問題項目ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivitySurveyQuestionItemID { get; set; }

    /// <summary>管理者活動問卷問題-問題項目名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者活動問卷問題-問題項目排序</summary>
    [JsonPropertyName("c")]
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}