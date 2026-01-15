namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得全部管理者部門-回應項目模型</summary>
public class MbsBscLgcGetAllDepartmentRspItemMdl
{
    /// <summary>管理者部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }
}