namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

/// <summary>核心-員工-專案里程碑工項工作檔案-資料庫-新增-請求模型</summary>
public class CoEmpPsjwfDbAddReqMdl
{
    /// <summary>員工專案ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項工作ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>檔案網址</summary>
    public string EmployeeProjectStoneJobWorkFileUrl { get; set; }
}
