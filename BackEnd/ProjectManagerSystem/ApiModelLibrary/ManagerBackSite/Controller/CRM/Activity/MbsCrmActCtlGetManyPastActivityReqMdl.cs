using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆客戶過往活動-請求模型</summary>
public class MbsCrmActCtlGetManyPastActivityReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("b")]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageSize { get; set; }
}
