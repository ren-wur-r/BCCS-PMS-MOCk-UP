namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-取得多筆數量從工項ID列表-回應項目模型</summary>
public class CoEmpPsjeDbGetManyCountFromTaskIdListRspItemMdl
{
    /// <summary>專案里程碑工項ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>執行者數量</summary>
    public int ExecutorCount { get; set; }
}
