namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoEmpPrjDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>員工專案-ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案-名稱</summary>
    public string EmployeeProjectName { get; set; }
}