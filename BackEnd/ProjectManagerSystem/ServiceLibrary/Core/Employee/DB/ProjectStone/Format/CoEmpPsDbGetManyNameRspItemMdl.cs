namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-取得多筆[名稱]-回應項目模型</summary>
public class CoEmpPsDbGetManyNameRspItemMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }
}