namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司子分類-項目-回應模型</summary>
public class MbsBscLgcGetManyCompanySubKindRspItemMdl
{
    /// <summary>公司子分類ID</summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>公司子分類名稱</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>公司子分類是否啟用</summary>
    public bool ManagerCompanySubKindIsEnable { get; set; }
}
