namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆完成狀態從工項ID-回應項目模型</summary>
public class CoEmpPsjbDbGetManyIsFinishFromTaskIdRspItemMdl
{
    /// <summary>專案里程碑工項清單ID</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
