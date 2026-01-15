using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得商機產品-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    [JsonPropertyName("b")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品主分類名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品子分類ID</summary>
    [JsonPropertyName("d")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品子分類名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者產品ID</summary>
    [JsonPropertyName("f")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品規格ID</summary>
    [JsonPropertyName("h")]
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者產品規格名稱</summary>
    [JsonPropertyName("i")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>商機產品售價</summary>
    [JsonPropertyName("j")]
    public decimal EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品成交價</summary>
    [JsonPropertyName("k")]
    public decimal EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品成本</summary>
    [JsonPropertyName("l")]
    public decimal EmployeePipelineProductCostPrice { get; set; }

    /// <summary>商機產品數量</summary>
    [JsonPropertyName("m")]
    public decimal EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品新購/續約</summary>
    [JsonPropertyName("n")]
    public DbAtomEmployeePipelineProductPurchaseKindEnum EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>商機產品採購方式</summary>
    [JsonPropertyName("o")]
    public DbAtomEmployeePipelineProductContractKindEnum EmployeePipelineProductContractKindID { get; set; }

    /// <summary>商機產品採購文字</summary>
    [JsonPropertyName("p")]
    public string EmployeePipelineProductContractText { get; set; }
}