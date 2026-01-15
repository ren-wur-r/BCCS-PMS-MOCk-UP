using DataModelLibrary.Database.AtomEmployeePipelineProduct;

namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-取得-回應模型</summary>
public class CoEmpPplPrdDbGetRspMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>商機產品售價</summary>
    public decimal EmployeePipelineProductSellPrice { get; set; }

    /// <summary>商機產品成交價</summary>
    public decimal EmployeePipelineProductClosingPrice { get; set; }

    /// <summary>商機產品成本</summary>
    public decimal EmployeePipelineProductCostPrice { get; set; }

    /// <summary>商機產品數量</summary>
    public decimal EmployeePipelineProductCount { get; set; }

    /// <summary>商機產品新購/續約</summary>
    public DbAtomEmployeePipelineProductPurchaseKindEnum EmployeePipelineProductPurchaseKindID { get; set; }

    /// <summary>商機產品採購方式</summary>
    public DbAtomEmployeePipelineProductContractKindEnum EmployeePipelineProductContractKindID { get; set; }

    /// <summary>商機產品採購文字</summary>
    public string EmployeePipelineProductContractText { get; set; }
}