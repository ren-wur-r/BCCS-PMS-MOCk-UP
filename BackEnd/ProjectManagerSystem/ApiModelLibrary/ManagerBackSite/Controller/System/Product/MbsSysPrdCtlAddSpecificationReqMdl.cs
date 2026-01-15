using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增規格-請求模型</summary>
public class MbsSysPrdCtlAddSpecificationReqMdl : MbsCtlBaseReqMdl
{

    /// <summary>管理者-產品ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    [JsonPropertyName("b")]
    [Required]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    [JsonPropertyName("c")]
    [Required]
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    [JsonPropertyName("d")]
    [Required]
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    [JsonPropertyName("e")]
    [Required]
    public bool ManagerProductSpecificationIsEnable { get; set; }
}