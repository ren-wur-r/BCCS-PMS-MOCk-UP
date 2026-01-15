using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

/// <summary>管理者後台-系統設定-角色-取得多筆-項目-回應模型</summary>
public class MbsSysRolLgcGetManyItemRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    public bool ManagerRoleIsEnable { get; set; }

    /// <summary>員工-人數</summary>
    public int EmployeeCount { get; set; }
}
