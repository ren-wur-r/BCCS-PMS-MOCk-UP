namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得-請求模型</summary>
public class CoEmpPrjDbGetReqMdl
{
    /// <summary>員工專案ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工商機ID</summary>
    public int? EmployeePipelineID { get; set; }

}