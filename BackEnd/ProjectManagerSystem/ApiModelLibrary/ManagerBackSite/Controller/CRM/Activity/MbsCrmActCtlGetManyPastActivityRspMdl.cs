using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆客戶過往活動-回應模型</summary>
public class MbsCrmActCtlGetManyPastActivityRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>最新過往活動</summary>
    [JsonPropertyName("a")]
    public MbsCrmActCtlGetManyPastActivityRspItemMdl LatestPastActivity { get; set; }

    /// <summary>過往活動列表</summary>
    [JsonPropertyName("b")]
    public List<MbsCrmActCtlGetManyPastActivityRspItemMdl> PastActivityList { get; set; }

    /// <summary>過往活動總數</summary>
    [JsonPropertyName("c")]
    public int TotalCount { get; set; }
}
