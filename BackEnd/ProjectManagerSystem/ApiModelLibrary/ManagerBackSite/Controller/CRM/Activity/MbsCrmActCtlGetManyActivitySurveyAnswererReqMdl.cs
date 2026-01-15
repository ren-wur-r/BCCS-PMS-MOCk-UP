using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆活動問卷回答者-請求模型</summary>
public class MbsCrmActCtlGetManyActivitySurveyAnswererReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷回答者-公司名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerActivitySurveyAnswererCompanyName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerActivitySurveyAnswererContacterName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口信箱</summary>
    [JsonPropertyName("d")]
    public string ManagerActivitySurveyAnswererContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("e")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [Required]
    [JsonPropertyName("f")]
    public int PageSize { get; set; }
}