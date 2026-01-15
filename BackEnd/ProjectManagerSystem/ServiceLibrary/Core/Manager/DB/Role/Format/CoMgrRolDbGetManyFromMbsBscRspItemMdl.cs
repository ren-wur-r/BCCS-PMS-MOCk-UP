namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoMgrRolDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-地區ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>角色-是否啟用</summary>
    public bool ManagerRoleIsEnable { get; set; }
}