namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆管理者產品主分類-回應項目模型</summary>
public class MbsBscLgcGetManyProductMainKindRspItemMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    public bool ManagerProductMainKindIsEnable { get; set; }

}