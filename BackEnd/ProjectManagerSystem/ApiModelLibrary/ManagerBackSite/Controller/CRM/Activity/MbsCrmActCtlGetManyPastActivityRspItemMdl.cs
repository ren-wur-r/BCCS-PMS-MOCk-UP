using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆客戶過往活動-項目-回應模型</summary>
public class MbsCrmActCtlGetManyPastActivityRspItemMdl
{
    /// <summary>管理者活動名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset ManagerActivityEndTime { get; set; }
}
