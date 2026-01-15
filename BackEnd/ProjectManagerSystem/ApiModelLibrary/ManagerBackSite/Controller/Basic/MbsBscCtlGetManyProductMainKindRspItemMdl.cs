using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-產品主分類-取得多筆-回應項目模型</summary>
public class MbsBscCtlGetManyProductMainKindRspItemMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerProductMainKindIsEnable { get; set; }
}