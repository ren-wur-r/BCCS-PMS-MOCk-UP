using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-取得多筆關係公司-回應模型</summary>
public class MbsSysCmpLgcGetManyCompanyAffiliateRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者關係公司-列表</summary>
    public List<MbsSysCmpLgcGetManyCompanyAffiliateRspItemMdl> ManagerCompanyAffiliateList { get; set; }
}
