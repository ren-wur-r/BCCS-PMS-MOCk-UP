using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.ManagerActivitySurveyQuestion;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;
public class MbsCrmActCtlAddActivitySurveyQuestionReqItemQuestionMdl
{
    /// <summary>管理者-活動問卷問題-活動ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerActivityID { get; set; }

    /// <summary>管理者-活動問卷問題-問題類型</summary>
    [JsonPropertyName("b")]
    [Required]
    public DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者-活動問卷問題-問題標題</summary>
    [JsonPropertyName("c")]
    [Required]
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者-活動問卷問題-問題內容</summary>
    [JsonPropertyName("d")]
    [Required]
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>管理者-活動問卷問題-是否為其他</summary>
    [JsonPropertyName("e")]
    [Required]
    public bool IsOther { get; set; }

    /// <summary>管理者-活動問卷問題-排序</summary>
    [JsonPropertyName("f")]
    [Required]
    public short ManagerActivitySurveyQuestionSort { get; set; }

    /// <summary>管理者-活動問卷問題項目列表</summary>
    [JsonPropertyName("g")]
    [Required]
    public List<MbsCrmActCtlAddActivitySurveyQuestionReqItemQuestionItemMdl> ManagerActivitySurveyQuestionItemList { get; set; }
}
