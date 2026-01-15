using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動贊助-控制器-取得-回應模型</summary>
public class MbsCrmActCtlGetActivitySponsorRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動贊助商-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    [Required]
    [JsonPropertyName("b")]
    public decimal ManagerActivitySponsorAmount { get; set; }
}
