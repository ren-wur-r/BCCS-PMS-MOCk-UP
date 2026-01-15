using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆-回應項目模型</summary>
public class MbsSysPrdCtlGetManyRspItemMdl
{
    /// <summary>管理者-產品-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-主分類名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品-子分類名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品-是否為主力產品</summary>
    [JsonPropertyName("e")]
    public bool ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    [JsonPropertyName("f")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    [JsonPropertyName("g")]
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    [JsonPropertyName("h")]
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    [JsonPropertyName("i")]
    public bool ManagerProductSpecificationIsEnable { get; set; }
}