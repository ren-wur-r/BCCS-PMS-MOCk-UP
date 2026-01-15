using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆客戶過往活動-回應模型</summary>
public class MbsCrmActLgcGetManyPastActivityRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>最新過往活動</summary>
    public MbsCrmActLgcGetManyPastActivityRspItemMdl LatestPastActivity { get; set; }

    /// <summary>過往活動列表</summary>
    public List<MbsCrmActLgcGetManyPastActivityRspItemMdl> PastActivityList { get; set; }

    /// <summary>過往活動總數</summary>
    public int TotalCount { get; set; }
}