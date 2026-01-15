namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

/// <summary>核心-員工-商機窗口-資料庫-移除多筆根據公司ID不匹配-請求模型</summary>
public class CoEmpPplContDbRemoveManyByCompanyIdMismatchReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }
}
