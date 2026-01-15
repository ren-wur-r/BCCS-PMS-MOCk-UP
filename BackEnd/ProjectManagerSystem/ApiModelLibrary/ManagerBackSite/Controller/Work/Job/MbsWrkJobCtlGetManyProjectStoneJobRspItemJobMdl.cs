using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項-回應項目工項模型</summary>
public class MbsWrkJobCtlGetManyProjectStoneJobRspItemJobMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工專案-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    [JsonPropertyName("d")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-狀態</summary>
    [JsonPropertyName("e")]
    public DbAtomEmployeeProjectStatusEnum EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    [JsonPropertyName("g")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-執行者-列表</summary>
    [JsonPropertyName("h")]
    public List<MbsWrkJobCtlGetManyProjectStoneJobRspItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

}