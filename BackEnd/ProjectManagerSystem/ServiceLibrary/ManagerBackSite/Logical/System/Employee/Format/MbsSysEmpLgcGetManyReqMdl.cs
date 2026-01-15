using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

/// <summary>管理者後台-系統設定-員工-取得多筆-請求模型</summary>
public class MbsSysEmpLgcGetManyReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-部門ID</summary>
    public int? ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色ID</summary>
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool? EmployeeIsEnable { get; set; }

    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}