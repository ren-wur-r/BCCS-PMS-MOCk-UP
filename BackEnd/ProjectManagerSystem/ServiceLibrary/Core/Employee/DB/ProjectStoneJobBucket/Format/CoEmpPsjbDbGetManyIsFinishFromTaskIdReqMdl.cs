namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆完成狀態從工項ID-請求模型</summary>
public class CoEmpPsjbDbGetManyIsFinishFromTaskIdReqMdl
{
    /// <summary>專案里程碑工項ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }
}
