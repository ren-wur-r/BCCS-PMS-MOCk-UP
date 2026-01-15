using System;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobCtlGetManyProjectStoneJobWorkReqMdl : MbsCtlBaseReqMdl
{

    /// <summary>員工專案-ID</summary>
    [JsonPropertyName("a")]
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    [JsonPropertyName("b")]
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項-ID</summary>
    [JsonPropertyName("c")]
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-ID(保留欄位，未使用)</summary>
    [JsonPropertyName("d")]
    public int? EmployeeID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset? StartEmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset? EndEmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>頁面索引</summary>
    [JsonPropertyName("g")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [JsonPropertyName("h")]
    public int PageSize { get; set; }
}