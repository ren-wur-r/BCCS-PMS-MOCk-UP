using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動支出-控制器-更新-請求模型</summary>
public class MbsCrmActCtlUpdateActivityExpenseReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動支出ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityExpenseID { get; set; }

    /// <summary>管理者活動支出-項目</summary>
    [JsonPropertyName("b")]
    [StringLength(30, ErrorMessage = "Invalid parameters")]
    public string ManagerActivityExpenseItem { get; set; }

    /// <summary>管理者活動支出-數量</summary>
    [JsonPropertyName("c")]
    public int? ManagerActivityExpenseQuantity { get; set; }

    /// <summary>管理者活動支出-總金額</summary>
    [JsonPropertyName("d")]
    public decimal? ManagerActivityExpenseTotalAmount { get; set; }
}
