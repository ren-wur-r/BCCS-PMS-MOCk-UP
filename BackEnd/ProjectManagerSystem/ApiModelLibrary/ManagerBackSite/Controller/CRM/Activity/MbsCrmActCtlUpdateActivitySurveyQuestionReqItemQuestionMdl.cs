using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.ManagerActivitySurveyQuestion;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-更新活動問卷問題-問題項目請求模型</summary>
public class MbsCrmActCtlUpdateActivitySurveyQuestionReqItemQuestionMdl
{
    /// <summary>管理者活動問卷問題類型</summary>
    [JsonPropertyName("a")]
    [Required]
    public DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者活動問卷問題標題</summary>
    [JsonPropertyName("b")]
    [Required]
    [StringLength(30)]
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者活動問卷問題內容</summary>
    [JsonPropertyName("c")]
    [Required]
    [StringLength(30)]
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>是否為其他</summary>
    [JsonPropertyName("d")]
    public bool IsOther { get; set; }

    /// <summary>管理者活動問卷問題排序</summary>
    [JsonPropertyName("e")]
    [Required]
    public short ManagerActivitySurveyQuestionSort { get; set; }

    /// <summary>管理者活動問卷問題項目列表</summary>
    [JsonPropertyName("f")]
    public List<MbsCrmActCtlUpdateActivitySurveyQuestionReqItemQuestionItemMdl> ManagerActivitySurveyQuestionItemList { get; set; }
}