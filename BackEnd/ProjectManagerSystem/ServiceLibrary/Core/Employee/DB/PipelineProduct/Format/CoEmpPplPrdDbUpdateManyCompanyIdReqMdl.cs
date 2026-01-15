namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

/// <summary>核心-員工-商機產品-資料庫-更新多筆公司ID-請求模型</summary>
public class CoEmpPplPrdDbUpdateManyCompanyIdReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }
}