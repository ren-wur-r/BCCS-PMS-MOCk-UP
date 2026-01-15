using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-更新規格-請求模型</summary>
public class MbsSysPrdCtlUpdateSpecificationReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-ID</summary>
    [JsonPropertyName("b")]
    [Required]
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    [JsonPropertyName("d")]
    public decimal? ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    [JsonPropertyName("e")]
    public decimal? ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    [JsonPropertyName("f")]
    public bool? ManagerProductSpecificationIsEnable { get; set; }
}