using DataModelLibrary.Database.AtomEmployeePipelineProduct;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機-產品項目-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineReqProductItemMdl : MbsLgcBaseReqMdl
{
    /// <summary>經理人產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>經理人產品規格ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>商機產品售價</summary>
    public decimal EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品成交價</summary>
    public decimal EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品成本價</summary>
    public decimal EmployeePipelineProductCostPrice { get; set; }

    /// <summary>經理人產品主分類佣金比例</summary>
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>商機產品數量</summary>
    public decimal EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品購買類型</summary>
    public DbAtomEmployeePipelineProductPurchaseKindEnum EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>商機產品合約類型</summary>
    public DbAtomEmployeePipelineProductContractKindEnum EmployeePipelineProductContractKindID { get; set; }

    /// <summary>商機產品合約文本</summary>
    public string EmployeePipelineProductContractText { get; set; }
}
