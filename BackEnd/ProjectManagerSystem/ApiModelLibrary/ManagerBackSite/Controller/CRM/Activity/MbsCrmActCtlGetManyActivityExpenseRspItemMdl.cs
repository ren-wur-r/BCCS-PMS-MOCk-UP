using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動支出-控制器-取得多筆-回應項目模型</summary>
public class MbsCrmActCtlGetManyActivityExpenseRspItemMdl
{
    /// <summary>管理者活動支出ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityExpenseID { get; set; }

    /// <summary>管理者活動支出-項目</summary>
    [Required]
    [JsonPropertyName("b")]
    public string ManagerActivityExpenseItem { get; set; }

    /// <summary>管理者活動支出-數量</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerActivityExpenseQuantity { get; set; }

    /// <summary>管理者活動支出-總金額</summary>
    [Required]
    [JsonPropertyName("d")]
    public decimal ManagerActivityExpenseTotalAmount { get; set; }
}
