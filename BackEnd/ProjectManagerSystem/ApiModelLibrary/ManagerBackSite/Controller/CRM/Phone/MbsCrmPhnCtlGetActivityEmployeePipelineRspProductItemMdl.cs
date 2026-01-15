using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得活動名單-產品項目-回應模型</summary>
public class MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl
{
    /// <summary>商機產品ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }

    /// <summary>產品ID</summary>
    [JsonPropertyName("b")]
    public int ManagerProductID { get; set; }

    /// <summary>產品名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductName { get; set; }

    /// <summary>產品主分類ID</summary>
    [JsonPropertyName("d")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>產品主分類名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>產品子分類ID</summary>
    [JsonPropertyName("f")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>產品子分類名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>產品規格ID</summary>
    [JsonPropertyName("h")]
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>產品規格名稱</summary>
    [JsonPropertyName("i")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>商機產品-售價</summary>
    [JsonPropertyName("j")]
    public decimal ManagerProductSpecificationSellPrice { get; set; }
}
