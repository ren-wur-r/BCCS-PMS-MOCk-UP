namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-系統-角色]-回應項目模型</summary>
public class CoMgrRolDbGetManyFromMbsSysRolRspItemMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    public bool ManagerRoleIsEnable { get; set; }

}