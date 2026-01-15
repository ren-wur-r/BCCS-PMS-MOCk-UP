using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-更新活動問卷問題-問題項目請求模型</summary>
public class MbsCrmActCtlUpdateActivitySurveyQuestionReqItemQuestionItemMdl
{
    /// <summary>管理者活動問卷問題項目名稱</summary>
    [JsonPropertyName("a")]
    [Required]
    [StringLength(15)]
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者活動問卷問題項目排序</summary>
    [JsonPropertyName("b")]
    [Required]
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}