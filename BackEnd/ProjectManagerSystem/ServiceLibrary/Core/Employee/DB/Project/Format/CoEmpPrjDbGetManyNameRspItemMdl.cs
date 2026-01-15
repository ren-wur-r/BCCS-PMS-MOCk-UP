namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得多筆[名稱]-回應項目模型</summary>
public class CoEmpPrjDbGetManyNameRspItemMdl
{
    /// <summary>員工-專案-ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工-專案-名稱</summary>
    public string EmployeeProjectName { get; set; }
}