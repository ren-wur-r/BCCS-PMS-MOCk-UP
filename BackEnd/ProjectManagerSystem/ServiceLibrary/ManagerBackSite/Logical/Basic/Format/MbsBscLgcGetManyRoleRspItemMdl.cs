namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者區域-取得多筆-回應項目模型</summary>
public class MbsBscLgcGetManyRoleRspItemMdl
{
    /// <summary>管理者角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>管理者角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者角色-區域ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者角色-區域名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者角色-部門名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者角色-是否啟用</summary>
    public bool ManagerRoleIsEnable { get; set; }
}