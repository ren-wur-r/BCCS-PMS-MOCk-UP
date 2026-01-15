using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-商機產品項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl
{
    /// <summary>商機產品ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品主分類名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品子分類名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者產品規格名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>商機產品售價</summary>
    [JsonPropertyName("f")]
    public decimal EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品成交價</summary>
    [JsonPropertyName("g")]
    public decimal EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品成本</summary>
    [JsonPropertyName("h")]
    public decimal EmployeePipelineProductCostPrice { get; set; }

    /// <summary>商機產品毛利</summary>
    [JsonPropertyName("i")]
    public decimal EmployeePipelineProductGrossProfit { get; set; }

    /// <summary>商機產品數量</summary>
    [JsonPropertyName("j")]
    public decimal EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品新購/續約</summary>
    [JsonPropertyName("k")]
    public DbAtomEmployeePipelineProductPurchaseKindEnum EmployeePipelineProductPurchaseKind { get; set; }

    /// <summary>商機產品採購方式</summary>
    [JsonPropertyName("l")]
    public DbAtomEmployeePipelineProductContractKindEnum EmployeePipelineProductContractKind { get; set; }

    /// <summary>商機產品採購文字</summary>
    [JsonPropertyName("m")]
    public string EmployeePipelineProductContractText { get; set; }

    /// <summary>管理者產品主分類-業務毛利率</summary>
    [JsonPropertyName("n")]
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者產品ID</summary>
    [JsonPropertyName("o")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    [JsonPropertyName("p")]
    public int ManagerProductSpecificationID { get; set; }
}
