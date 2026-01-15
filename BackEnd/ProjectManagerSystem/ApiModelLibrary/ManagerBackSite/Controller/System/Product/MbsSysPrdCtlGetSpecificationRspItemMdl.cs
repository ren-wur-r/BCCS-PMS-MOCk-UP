using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得規格-回應項目模型</summary>
public class MbsSysPrdCtlGetSpecificationRspItemMdl
{
    /// <summary>管理者-產品規格-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    [JsonPropertyName("c")]
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    [JsonPropertyName("d")]
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    [JsonPropertyName("e")]
    public bool ManagerProductSpecificationIsEnable { get; set; }
}