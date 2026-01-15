namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者部門-取得多筆-回應項目模型</summary>
public class MbsBscLgcGetManyDepartmentRspItemMdl
{
    /// <summary>管理者部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者部門-是否啟用</summary>
    public bool ManagerDepartmentIsEnable { get; set; }
}