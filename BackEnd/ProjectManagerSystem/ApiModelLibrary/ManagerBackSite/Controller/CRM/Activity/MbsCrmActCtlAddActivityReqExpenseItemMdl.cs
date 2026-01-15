using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增-支出項目-請求模型</summary>
public class MbsCrmActCtlAddActivityReqExpenseItemMdl
{
    /// <summary>管理者活動支出-項目</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(30, ErrorMessage = "Invalid parameters")]
    public string ManagerActivityExpenseItem { get; set; }

    /// <summary>管理者活動支出-數量</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerActivityExpenseQuantity { get; set; }

    /// <summary>管理者活動支出-總金額</summary>
    [Required]
    [JsonPropertyName("c")]
    public decimal ManagerActivityExpenseTotalAmount { get; set; }
}
