namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

public class CoEmpPsjeDbAddManyReqItemMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案里程碑ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑工項ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }
}