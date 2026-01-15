using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆活動-回應模型</summary>
public class MbsCrmPhnCtlGetManyActivityRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>活動列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPhnCtlGetManyActivityRspItemMdl> ManagerActivityList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
