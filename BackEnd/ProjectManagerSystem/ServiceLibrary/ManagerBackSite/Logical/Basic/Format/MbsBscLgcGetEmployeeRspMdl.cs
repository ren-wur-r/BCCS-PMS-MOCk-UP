using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得員工資訊-回應模型</summary>
public class MbsBscLgcGetEmployeeRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>員工-名稱</summary>
    public string EmployeeName { get; set; }

    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者後台-目錄權限列表</summary>
    public List<MbsBscLgcGetEmployeeRspItemMdl> ManagerBackSiteMenuPermissionList { get; set; }
}