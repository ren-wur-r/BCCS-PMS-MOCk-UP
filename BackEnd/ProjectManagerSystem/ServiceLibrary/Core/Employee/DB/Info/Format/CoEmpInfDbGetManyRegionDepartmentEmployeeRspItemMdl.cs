namespace ServiceLibrary.Core.Employee.DB.Info.Format;

public class CoEmpInfDbGetManyRegionDepartmentEmployeeRspItemMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>管理者地區名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者部門名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>員工名稱</summary>
    public string EmployeeName { get; set; }

}