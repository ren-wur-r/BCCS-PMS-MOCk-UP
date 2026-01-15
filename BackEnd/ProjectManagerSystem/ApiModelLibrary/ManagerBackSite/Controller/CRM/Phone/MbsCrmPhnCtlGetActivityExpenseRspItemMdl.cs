using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得單筆活動-支出-回應項目模型</summary>
public class MbsCrmPhnCtlGetActivityExpenseRspItemMdl
{
    /// <summary>管理者活動支出ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivityExpenseID { get; set; }

    /// <summary>管理者活動支出-項目</summary>
    [JsonPropertyName("b")]
    public string ManagerActivityExpenseItem { get; set; }

    /// <summary>管理者活動支出-數量</summary>
    [JsonPropertyName("c")]
    public int ManagerActivityExpenseQuantity { get; set; }

    /// <summary>管理者活動支出-總金額</summary>
    [JsonPropertyName("d")]
    public decimal ManagerActivityExpenseTotalAmount { get; set; }
}
