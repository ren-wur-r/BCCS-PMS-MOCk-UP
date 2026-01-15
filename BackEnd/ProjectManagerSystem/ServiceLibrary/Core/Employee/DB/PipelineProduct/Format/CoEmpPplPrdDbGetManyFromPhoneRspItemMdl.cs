namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-取得多筆從[管理者後台-CRM-電銷管理]-回應項目</summary>
public class CoEmpPplPrdDbGetManyFromPhoneRspItemMdl
{
    /// <summary>商機產品ID</summary>
    public int EmployeePipelineProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品子分類名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者產品規格名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者產品規格售價</summary>
    public decimal ManagerProductSpecificationSellPrice { get; set; }

}