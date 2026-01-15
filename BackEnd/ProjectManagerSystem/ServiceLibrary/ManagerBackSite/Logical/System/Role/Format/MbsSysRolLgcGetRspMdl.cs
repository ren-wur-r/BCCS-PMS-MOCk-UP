using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

/// <summary>管理者後台-系統設定-角色-取得-回應模型</summary>
public class MbsSysRolLgcGetRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-角色-地區名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-角色-部門ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-部門名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-角色-備註</summary>
    public string ManagerRoleRemark { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    public bool ManagerRoleIsEnable { get; set; }

    /// <summary>員工-人數</summary>
    public int EmployeeCount { get; set; }

    /// <summary>管理者-角色權限列表</summary>
    public List<MbsSysRolLgcGetRspItemMdl> ManagerRolePermissionList { get; set; }
}