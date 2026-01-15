using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.ManagerActivitySurveyQuestion;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得活動問卷問題-回應項目模型</summary>
public class MbsCrmActCtlGetActivitySurveyQuestionRspItemMdl
{
    /// <summary>管理者活動問卷問題-問題ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>管理者活動問卷問題-問題類型ID</summary>
    [JsonPropertyName("b")]
    public DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者活動問卷問題-問題標題</summary>
    [JsonPropertyName("c")]
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者活動問卷問題-問題內容</summary>
    [JsonPropertyName("d")]
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>是否為其他</summary>
    [JsonPropertyName("e")]
    public bool IsOther { get; set; }

    /// <summary>管理者活動問卷問題-問題排序</summary>
    [JsonPropertyName("f")]
    public short ManagerActivitySurveyQuestionSort { get; set; }

    /// <summary>管理者活動問卷問題-問題項目列表</summary>
    [JsonPropertyName("g")]
    public List<MbsCrmActCtlGetActivitySurveyQuestionRspItemQuestionItemMdl> ManagerActivitySurveyQuestionItemList { get; set; }
}