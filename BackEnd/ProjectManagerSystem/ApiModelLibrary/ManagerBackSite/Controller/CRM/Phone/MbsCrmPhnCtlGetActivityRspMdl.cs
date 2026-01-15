using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得單筆活動-回應模型</summary>
public class MbsCrmPhnCtlGetActivityRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>活動名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動類型</summary>
    [JsonPropertyName("b")]
    public DbAtomManagerActivityKindEnum ManagerActivityKind { get; set; }

    /// <summary>活動開始時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>活動結束時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-地點</summary>
    [JsonPropertyName("e")]
    public string ManagerActivityLocation { get; set; }

    /// <summary>管理者活動-期望名單數</summary>
    [JsonPropertyName("f")]
    public int ManagerActivityExpectedLeadCount { get; set; }

    /// <summary>管理者活動-內容</summary>
    [JsonPropertyName("g")]
    public string ManagerActivityContent { get; set; }

    /// <summary>管理者活動-實際名單數</summary>
    [JsonPropertyName("h")]
    public int ManagerActivityRegisteredCount { get; set; }

    /// <summary>管理者活動-已轉電銷數</summary>
    [JsonPropertyName("i")]
    public int ManagerActivityTransferredToSalesCount { get; set; }

    /// <summary>管理者活動-商機數</summary>
    [JsonPropertyName("j")]
    public int ManagerActivityEmployeePipelineCount { get; set; }

    /// <summary>管理者活動-已撥打數(電銷紀錄數)</summary>
    [JsonPropertyName("k")]
    public int ManagerActivityPhoneCount { get; set; }

    /// <summary>管理者活動產品列表</summary>
    [JsonPropertyName("l")]
    public List<MbsCrmPhnCtlGetActivityProductRspItemMdl> ManagerActivityProductList { get; set; }

    /// <summary>管理者活動贊助商列表</summary>
    [JsonPropertyName("m")]
    public List<MbsCrmPhnCtlGetActivitySponsorRspItemMdl> ManagerActivitySponsorList { get; set; }

    /// <summary>管理者活動支出列表</summary>
    [JsonPropertyName("n")]
    public List<MbsCrmPhnCtlGetActivityExpenseRspItemMdl> ManagerActivityExpenseList { get; set; }
}
