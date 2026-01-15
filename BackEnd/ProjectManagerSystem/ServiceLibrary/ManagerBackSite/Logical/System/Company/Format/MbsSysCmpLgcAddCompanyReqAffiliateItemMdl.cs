namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司-關係公司項目模型</summary>
public class MbsSysCmpLgcAddCompanyReqAffiliateItemMdl
{
    /// <summary>統一編號</summary>
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    public string ManagerCompanyAffiliateName { get; set; }
}
