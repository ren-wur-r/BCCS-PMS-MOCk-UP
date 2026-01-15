using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得單筆活動-贊助商-回應項目模型</summary>
public class MbsCrmPhnCtlGetActivitySponsorRspItemMdl
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
