using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增商機產品-請求模型</summary>
public class MbsCrmPplCtlAddEmployeePipelineProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者產品ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>商機產品-售價</summary>
    [Required]
    [JsonPropertyName("d")]
    public decimal EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品-成交價</summary>
    [Required]
    [JsonPropertyName("e")]
    public decimal EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品-成本</summary>
    [Required]
    [JsonPropertyName("f")]
    public decimal EmployeePipelineProductCostPrice { get; set; }

    /// <summary>商機產品-數量</summary>
    [Required]
    [JsonPropertyName("g")]
    public decimal EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品-新購/續約</summary>
    [Required]
    [JsonPropertyName("h")]
    public DbAtomEmployeePipelineProductPurchaseKindEnum EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>商機產品-採購方式</summary>
    [Required]
    [JsonPropertyName("i")]
    public DbAtomEmployeePipelineProductContractKindEnum EmployeePipelineProductContractKindID { get; set; }

    /// <summary>商機產品-採購文字</summary>
    [JsonPropertyName("j")]
    public string EmployeePipelineProductContractText { get; set; }
}