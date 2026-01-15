namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-更新-請求模型</summary>
public class CoEmpPsjbDbUpdateReqMdl
{
    /// <summary>專案里程碑工項清單ID</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>名稱</summary>
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>是否完成</summary>
    public bool? EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
