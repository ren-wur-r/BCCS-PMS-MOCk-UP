namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆窗口-回應項目模型</summary>
public class MbsBscLgcGetManyContacterRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口-名稱</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-電子郵件</summary>
    public string ManagerContacterEmail { get; set; }

}
