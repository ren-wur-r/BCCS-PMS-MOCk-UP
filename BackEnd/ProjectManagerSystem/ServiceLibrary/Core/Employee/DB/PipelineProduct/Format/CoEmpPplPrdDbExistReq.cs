namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-是否存在-請求模型</summary>
public class CoEmpPplPrdDbExistReqMdl
{
    /// <summary>商機ID</summary>
    public int? EmployeePipelineID { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int? ManagerProductSpecificationID { get; set; }
}