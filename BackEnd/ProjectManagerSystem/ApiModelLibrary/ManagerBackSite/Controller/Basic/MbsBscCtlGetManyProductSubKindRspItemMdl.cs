using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-產品子分類-取得多筆-回應項目模型</summary>
public class MbsBscCtlGetManyProductSubKindRspItemMdl
{
    /// <summary>產品子分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>產品子分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>產品子分類-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerProductSubKindIsEnable { get; set; }
}