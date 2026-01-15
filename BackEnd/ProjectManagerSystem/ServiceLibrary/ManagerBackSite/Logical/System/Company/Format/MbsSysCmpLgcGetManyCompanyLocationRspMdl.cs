using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-取得多筆公司營業地點-回應模型</summary>
public class MbsSysCmpLgcGetManyCompanyLocationRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者公司營業地點-列表</summary>
    public List<MbsSysCmpLgcGetManyCompanyLocationRspItemMdl> ManagerCompanyLocationList { get; set; }
}
