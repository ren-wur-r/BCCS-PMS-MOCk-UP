using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-展示層-取得多筆產品-回應項目</summary>
public class MbsBscCtlGetManyProductRspItemMdl
{
    /// <summary>管理者-產品-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerProductIsEnable { get; set; }

    /// <summary>管理者-產品-主類別ID</summary>
    [JsonPropertyName("d")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-主類別名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品-子類別ID</summary>
    [JsonPropertyName("f")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-子類別名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品-主分類-業務毛利率</summary>
    [JsonPropertyName("h")]
    public decimal ManagerProductMainKindCommissionRate { get; set; }
}
