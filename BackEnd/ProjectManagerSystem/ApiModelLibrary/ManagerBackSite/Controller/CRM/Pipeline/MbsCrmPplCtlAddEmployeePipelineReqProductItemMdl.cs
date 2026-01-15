using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>
/// 管理者後台-CRM-商機管理-控制器-新增商機-產品-請求項目模型
/// </summary>
public class MbsCrmPplCtlAddEmployeePipelineReqProductItemMdl
{
    /// <summary>
    /// 管理者產品ID
    /// </summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerProductID { get; set; }

    /// <summary>
    /// 管理者產品規格ID
    /// </summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>
    /// 員工商機產品銷售價格
    /// </summary>
    [Required]
    [JsonPropertyName("c")]
    public decimal EmployeePipelineProductSellPrice { get; set; }

    /// <summary>
    /// 員工商機產品成交價格
    /// </summary>
    [Required]
    [JsonPropertyName("d")]
    public decimal EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>
    /// 員工商機產品成本價格
    /// </summary>
    [Required]
    [JsonPropertyName("e")]
    public decimal EmployeePipelineProductCostPrice { get; set; }

    /// <summary>
    /// 員工商機產品數量
    /// </summary>
    [Required]
    [JsonPropertyName("f")]
    public decimal EmployeePipelineProductCount { get; set; }

    /// <summary>
    /// 員工商機產品採購類型ID
    /// </summary>
    [Required]
    [JsonPropertyName("g")]
    public DbAtomEmployeePipelineProductPurchaseKindEnum EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>
    /// 員工商機產品合約類型ID
    /// </summary>
    [Required]
    [JsonPropertyName("h")]
    public DbAtomEmployeePipelineProductContractKindEnum EmployeePipelineProductContractKindID { get; set; }

    /// <summary>
    /// 員工商機產品合約文字
    /// </summary>
    [JsonPropertyName("i")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string EmployeePipelineProductContractText { get; set; }
}
