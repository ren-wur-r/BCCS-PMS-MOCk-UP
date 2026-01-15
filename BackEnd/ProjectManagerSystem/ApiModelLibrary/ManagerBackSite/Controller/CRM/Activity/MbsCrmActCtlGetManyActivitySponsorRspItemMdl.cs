using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動贊助-控制器-取得多筆-回應項目模型</summary>
public class MbsCrmActCtlGetManyActivitySponsorRspItemMdl
{
    /// <summary>管理者活動贊助商ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivitySponsorID { get; set; }

    /// <summary>管理者活動贊助商-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    [JsonPropertyName("c")]
    public decimal ManagerActivitySponsorAmount { get; set; }
}
