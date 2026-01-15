namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-取得多筆關係公司-回應模型-項目</summary>
public class MbsSysCmpLgcGetManyCompanyAffiliateRspItemMdl
{
    /// <summary>管理者關係公司-ID</summary>
    public int ManagerCompanyAffiliateID { get; set; }

    /// <summary>管理者關係公司-統一編號</summary>
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    public string ManagerCompanyAffiliateName { get; set; }
}
