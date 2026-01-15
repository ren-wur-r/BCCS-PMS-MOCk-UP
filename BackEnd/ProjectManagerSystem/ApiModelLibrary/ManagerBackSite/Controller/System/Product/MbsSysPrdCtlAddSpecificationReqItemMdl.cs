using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增規格-請求項目模型</summary>
public class MbsSysPrdCtlAddSpecificationReqItemMdl
{
    /// <summary>管理者-產品規格-名稱</summary>
    [JsonPropertyName("a")]
    [Required]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    [JsonPropertyName("b")]
    [Required]
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    [JsonPropertyName("c")]
    [Required]
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    [JsonPropertyName("d")]
    [Required]
    public bool ManagerProductSpecificationIsEnable { get; set; }
}