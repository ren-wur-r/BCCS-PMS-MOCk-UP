namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司主分類-項目-回應模型</summary>
public class MbsBscLgcGetManyCompanyMainKindRspItemMdl
{
    /// <summary>公司主分類ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>公司主分類名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>公司主分類是否啟用</summary>
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}
