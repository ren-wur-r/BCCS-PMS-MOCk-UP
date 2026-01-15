using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆客戶過往活動-請求模型</summary>
public class MbsCrmPhnCtlGetManyPastActivityReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>頁數</summary>
    [Required]
    [JsonPropertyName("b")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageSize { get; set; }
}
