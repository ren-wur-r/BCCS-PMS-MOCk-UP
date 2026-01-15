using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增活動問卷問題-請求項目模型</summary>

public class MbsCrmActCtlAddActivitySurveyQuestionReqItemQuestionItemMdl
{
    /// <summary>管理者-活動問卷問題項目-名稱</summary>
    [JsonPropertyName("a")]
    [Required]
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者-活動問卷問題項目-排序</summary>
    [JsonPropertyName("b")]
    [Required]
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}