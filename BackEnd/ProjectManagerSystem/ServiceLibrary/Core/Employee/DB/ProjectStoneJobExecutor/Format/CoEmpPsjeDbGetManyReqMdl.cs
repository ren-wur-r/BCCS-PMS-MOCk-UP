namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-取得多筆-請求模型</summary>
public class CoEmpPsjeDbGetManyReqMdl
{
    /// <summary>員工-專案-ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }
}