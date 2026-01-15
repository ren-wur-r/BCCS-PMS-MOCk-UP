using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-更新商機產品-請求模型</summary>
public class MbsCrmPplCtlUpdateEmployeePipelineProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機產品ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    [JsonPropertyName("c")]
    public int? ManagerProductSpecificationID { get; set; }

    /// <summary>商機產品-售價</summary>
    [JsonPropertyName("d")]
    public decimal? EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品-成交價</summary>
    [JsonPropertyName("e")]
    public decimal? EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品-成本</summary>
    [JsonPropertyName("f")]
    public decimal? EmployeePipelineProductCostPrice { get; set; }

    /// <summary>商機產品-數量</summary>
    [JsonPropertyName("g")]
    public decimal? EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品-新購/續約</summary>
    [JsonPropertyName("h")]
    public DbAtomEmployeePipelineProductPurchaseKindEnum? EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>商機產品-採購方式</summary>
    [JsonPropertyName("i")]
    public DbAtomEmployeePipelineProductContractKindEnum? EmployeePipelineProductContractKindID { get; set; }

    /// <summary>商機產品-採購文字</summary>
    [JsonPropertyName("j")]
    public string EmployeePipelineProductContractText { get; set; }
}