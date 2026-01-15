using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆客戶過往活動-回應模型</summary>
public class MbsCrmPhnLgcGetManyPastActivityRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>最新過往活動</summary>
    public MbsCrmPhnLgcGetManyPastActivityRspItemMdl LatestPastActivity { get; set; }

    /// <summary>過往活動列表</summary>
    public List<MbsCrmPhnLgcGetManyPastActivityRspItemMdl> PastActivityList { get; set; }

    /// <summary>過往活動總數</summary>
    public int TotalCount { get; set; }
}