using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆狀態從里程碑ID-回應項目模型</summary>
public class CoEmpPsjDbGetManyStatusFromStoneIdRspItemMdl
{
    /// <summary>員工專案里程碑工項ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>原子-員工-專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }
}
