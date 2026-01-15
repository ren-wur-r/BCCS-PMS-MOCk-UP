using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動產品-控制器-取得多筆-回應項目模型</summary>
public class MbsCrmActCtlGetManyActivityProductRspItemMdl
{
    /// <summary>管理者活動產品ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    [JsonPropertyName("b")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品-主分類ID</summary>
    [JsonPropertyName("d")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品-主分類名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品-子分類ID</summary>
    [JsonPropertyName("f")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品-子分類名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerProductSubKindName { get; set; }
}
