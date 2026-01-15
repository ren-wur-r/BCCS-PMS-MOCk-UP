using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆-回應模型</summary>
public class MbsCrmActCtlGetManyActivityRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlGetManyActivityRspItemMdl> ManagerActivityList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
