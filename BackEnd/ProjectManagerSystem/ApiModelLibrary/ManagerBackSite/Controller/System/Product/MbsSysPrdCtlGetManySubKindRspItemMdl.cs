using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆子分類-回應項目模型</summary>
public class MbsSysPrdCtlGetManySubKindRspItemMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品子分類-ID</summary>
    [JsonPropertyName("c")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    [JsonPropertyName("e")]
    public decimal ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    [JsonPropertyName("f")]
    public bool ManagerProductSubKindIsEnable { get; set; }
}