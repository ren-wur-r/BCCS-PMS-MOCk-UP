using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-取得關係公司-請求模型</summary>
public class MbsSysCmpLgcGetCompanyAffiliateReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者關係公司-ID</summary>
    public int ManagerCompanyAffiliateID { get; set; }
}
