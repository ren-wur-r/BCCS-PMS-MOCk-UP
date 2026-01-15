using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆客戶過往活動-回應模型</summary>
public class MbsCrmPhnCtlGetManyPastActivityRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>最新過往活動</summary>
    [JsonPropertyName("a")]
    public MbsCrmPhnCtlGetManyPastActivityRspItemMdl LatestPastActivity { get; set; }

    /// <summary>過往活動列表</summary>
    [JsonPropertyName("b")]
    public List<MbsCrmPhnCtlGetManyPastActivityRspItemMdl> PastActivityList { get; set; }

    /// <summary>過往活動總數</summary>
    [JsonPropertyName("c")]
    public int TotalCount { get; set; }
}
