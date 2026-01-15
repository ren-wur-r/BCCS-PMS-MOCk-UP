using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

/// <summary>核心-管理者-關係公司-資料庫-取得多筆-回應模型</summary>
public class CoMgrCmpAffDbGetManyRspMdl
{
    /// <summary>管理者關係公司清單</summary>
    public List<CoMgrCmpAffDbGetManyRspItemMdl> ManagerCompanyAffiliateList { get; set; }
}