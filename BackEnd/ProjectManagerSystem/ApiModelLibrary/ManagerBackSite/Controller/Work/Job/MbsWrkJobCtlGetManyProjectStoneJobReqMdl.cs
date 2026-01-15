using System;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項-請求模型</summary>
public class MbsWrkJobCtlGetManyProjectStoneJobReqMdl : MbsCtlBaseReqMdl
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

    /// <summary>員工-專案里程碑工項-狀態</summary>
    [JsonPropertyName("d")]
    public DbAtomEmployeeProjectStatusEnum? EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>開始-員工-專案里程碑工項-訖止時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset? StartEmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>結束-員工-專案里程碑工項-訖止時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset? EndEmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>頁數</summary>
    [JsonPropertyName("g")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [JsonPropertyName("h")]
    public int PageSize { get; set; }

}