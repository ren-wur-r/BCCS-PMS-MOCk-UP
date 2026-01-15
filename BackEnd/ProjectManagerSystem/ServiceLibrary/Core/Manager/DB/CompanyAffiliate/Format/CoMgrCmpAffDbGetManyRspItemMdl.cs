namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

/// <summary>核心-管理者-關係公司-資料庫-取得多筆-回應項目模型</summary>
public class CoMgrCmpAffDbGetManyRspItemMdl
{
    /// <summary>管理者關係公司-ID</summary>
    public int ManagerCompanyAffiliateID { get; set; }

    /// <summary>統一編號</summary>
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    public string ManagerCompanyAffiliateName { get; set; }

    /// <summary>管理者關係公司-是否已刪除</summary>
    public bool ManagerCompanyAffiliateIsDeleted { get; set; }
}