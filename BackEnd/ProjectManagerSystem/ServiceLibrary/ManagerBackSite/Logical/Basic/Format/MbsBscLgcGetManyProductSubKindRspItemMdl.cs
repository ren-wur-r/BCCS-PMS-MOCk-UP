namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆管理者產品子分類-回應項目模型</summary>
public class MbsBscLgcGetManyProductSubKindRspItemMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool ManagerProductSubKindIsEnable { get; set; }
}