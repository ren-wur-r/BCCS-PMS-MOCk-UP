using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑-回應項目里程碑模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工-專案里程碑-前N日通知</summary>
    [JsonPropertyName("e")]
    public int EmployeeProjectStonePreNotifyDay { get; set; }

    /// <summary>原子員工專案-狀態</summary>
    [JsonPropertyName("f")]
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工-專案里程碑工項-列表</summary>
    [JsonPropertyName("g")]
    public List<MbsWrkPrjCtlGetManyProjectStoneRspItemJobMdl> EmployeeProjectStoneJobList { get; set; }
}