namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得-回應模型</summary>
public class CoMgrRolDbGetRspMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-角色-部門ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-備註</summary>
    public string ManagerRoleRemark { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    public bool ManagerRoleIsEnable { get; set; }
}