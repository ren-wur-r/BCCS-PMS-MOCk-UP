using DataModelLibrary.Database.AtomEmployeePipelineProduct;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-更新商機產品-請求模型</summary>
public class MbsCrmPplLgcUpdateEmployeePipelineProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機產品ID</summary>
    public int EmployeePipelineProductID { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品子分類ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int? ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int? ManagerProductSpecificationID { get; set; }

    /// <summary>商機產品-售價</summary>
    public decimal? EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品-成交價</summary>
    public decimal? EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品-成本</summary>
    public decimal? EmployeePipelineProductCostPrice { get; set; }

    /// <summary>商機產品-數量</summary>
    public decimal? EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品-新購/續約</summary>
    public DbAtomEmployeePipelineProductPurchaseKindEnum? EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>商機產品-採購方式</summary>
    public DbAtomEmployeePipelineProductContractKindEnum? EmployeePipelineProductContractKindID { get; set; }

    /// <summary>商機產品-採購文字</summary>
    public string EmployeePipelineProductContractText { get; set; }
}