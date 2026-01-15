using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-新增關係公司-回應模型</summary>
public class MbsSysCmpLgcAddCompanyAffiliateRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者關係公司-ID</summary>
    public int ManagerCompanyAffiliateID { get; set; }
}
