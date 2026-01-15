namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

/// <summary>核心-管理者-關係公司-資料庫-新增多筆-請求項目模型</summary>
public class CoMgrCmpAffDbAddManyReqItemMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    public string ManagerCompanyAffiliateName { get; set; }
}
