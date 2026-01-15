using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

/// <summary>管理者後台-系統設定-員工-新增-請求模型</summary>
public class MbsSysEmpLgcAddReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>員工-信箱</summary>
    public string EmployeeEmail { get; set; }

    /// <summary>員工-密碼</summary>
    public string EmployeePassword { get; set; }

    /// <summary>員工-名稱</summary>
    public string EmployeeName { get; set; }

    /// <summary>管理者-部門ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool EmployeeIsEnable { get; set; }

    /// <summary>員工-權限列表</summary>
    public List<MbsSysEmpLgcAddItemReqMdl> EmployeePermissionList { get; set; }

}