namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-基本]-項目-請求模型</summary>
public class CoEmpInfDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者-地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工-名稱</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool EmployeeIsEnable { get; set; }
}