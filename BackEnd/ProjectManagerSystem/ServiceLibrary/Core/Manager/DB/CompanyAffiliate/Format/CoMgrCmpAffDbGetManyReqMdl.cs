namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

/// <summary>核心-管理者-關係公司-資料庫-取得多筆-請求模型</summary>
public class CoMgrCmpAffDbGetManyReqMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者關係公司-是否已刪除</summary>
    public bool? ManagerCompanyAffiliateIsDeleted { get; set; }
}