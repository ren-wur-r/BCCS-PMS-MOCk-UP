using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆主分類-回應項目模型</summary>
public class MbsSysPrdCtlGetManyMainKindRspItemMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-業務毛利率</summary>
    [JsonPropertyName("c")]
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    [JsonPropertyName("d")]
    public bool ManagerProductMainKindIsEnable { get; set; }
}