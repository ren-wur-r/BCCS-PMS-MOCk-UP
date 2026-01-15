using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動贊助-控制器-新增-請求模型</summary>
public class MbsCrmActCtlAddActivitySponsorReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動贊助商-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    [StringLength(30, ErrorMessage = "Invalid parameters")]
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    [Required]
    [JsonPropertyName("c")]
    public decimal ManagerActivitySponsorAmount { get; set; }
}
