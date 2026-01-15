namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增-請求模型</summary>
public class CoEmpPsjeDbAddReqMdl
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