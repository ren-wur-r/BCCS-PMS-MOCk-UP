namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-系統-員工]-請求模型</summary>
public class CoEmpInfDbGetManyFromMbsSysEmpReqMdl
{
    /// <summary>管理者-部門-ID</summary>
    public int? ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-ID</summary>
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool? EmployeeIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}