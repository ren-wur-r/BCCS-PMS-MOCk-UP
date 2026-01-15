using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動-回應模型</summary>
public class MbsCrmPhnLgcGetManyActivityRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>活動列表</summary>
    public List<MbsCrmPhnLgcGetManyActivityRspItemMdl> ManagerActivityList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}