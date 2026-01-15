namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoEmpPsjDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>員工專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }
}