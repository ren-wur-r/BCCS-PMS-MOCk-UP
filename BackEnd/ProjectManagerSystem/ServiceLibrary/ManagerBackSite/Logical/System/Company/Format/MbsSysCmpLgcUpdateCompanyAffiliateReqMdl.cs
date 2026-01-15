using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-更新關係公司-請求模型</summary>
public class MbsSysCmpLgcUpdateCompanyAffiliateReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者關係公司-ID</summary>
    public int ManagerCompanyAffiliateID { get; set; }

    /// <summary>管理者關係公司-統一編號</summary>
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    public string ManagerCompanyAffiliateName { get; set; }

    /// <summary>管理者關係公司-是否已刪除</summary>
    public bool? ManagerCompanyAffiliateIsDeleted { get; set; }
}
