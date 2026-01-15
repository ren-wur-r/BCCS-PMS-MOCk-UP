using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆活動-回應項目模型</summary>
public class MbsCrmPhnCtlGetManyActivityRspItemMdl
{
    /// <summary>管理者活動ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動-類型</summary>
    [JsonPropertyName("c")]
    public DbAtomManagerActivityKindEnum ManagerActivityKind { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-實際名單數</summary>
    [JsonPropertyName("f")]
    public int ManagerActivityRegisteredCount { get; set; }

    /// <summary>管理者活動-已轉電銷數</summary>
    [JsonPropertyName("g")]
    public int ManagerActivityTransferredToSalesCount { get; set; }

    /// <summary>管理者活動-已撥打數</summary>
    [JsonPropertyName("h")]
    public int ManagerActivityPhoneCount { get; set; }

    /// <summary>管理者活動-商機數</summary>
    [JsonPropertyName("i")]
    public int ManagerActivityEmployeePipelineCount { get; set; }
}
