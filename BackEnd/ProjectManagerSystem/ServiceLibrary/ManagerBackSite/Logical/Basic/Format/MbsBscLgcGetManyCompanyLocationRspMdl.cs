using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司營業地點-回應模型</summary>
public class MbsBscLgcGetManyCompanyLocationRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者公司營業地點清單</summary>
    public List<MbsBscLgcGetManyCompanyLocationRspItemMdl> ManagerCompanyLocationList { get; set; }
}