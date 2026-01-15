namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

public class CoEmpPsjeDbRemoveManyReqMdl
{
    /// <summary>員工專案里程碑ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑工項ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }
}