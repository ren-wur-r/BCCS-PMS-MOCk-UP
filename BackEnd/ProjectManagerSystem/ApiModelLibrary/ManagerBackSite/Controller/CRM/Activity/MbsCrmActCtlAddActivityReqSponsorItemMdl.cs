using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增-贊助項目-請求模型</summary>
public class MbsCrmActCtlAddActivityReqSponsorItemMdl
{
    /// <summary>管理者活動贊助商-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(30, ErrorMessage = "Invalid parameters")]
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    [Required]
    [JsonPropertyName("b")]
    public decimal ManagerActivitySponsorAmount { get; set; }
}
