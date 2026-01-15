namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-新增多筆-請求項目模型</summary>
public class CoEmpPsjbDbAddManyReqItemMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案里程碑ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑工項ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>名稱</summary>
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
