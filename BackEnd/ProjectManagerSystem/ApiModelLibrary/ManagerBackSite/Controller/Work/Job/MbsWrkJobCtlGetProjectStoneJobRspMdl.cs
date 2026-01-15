using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項-回應模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-專案-名稱</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案-開始時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工-專案-結束時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    [JsonPropertyName("d")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    [JsonPropertyName("g")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    [JsonPropertyName("h")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    [JsonPropertyName("i")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-狀態</summary>
    [JsonPropertyName("j")]
    public DbAtomEmployeeProjectStatusEnum EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    [JsonPropertyName("k")]
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    [JsonPropertyName("l")]
    public List<MbsWrkJobCtlGetProjectStoneJobRspItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    [JsonPropertyName("m")]
    public List<MbsWrkJobCtlGetProjectStoneJobRspItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

    /// <summary>員工-專案里程碑工項工作-列表</summary>
    [JsonPropertyName("n")]
    public List<MbsWrkJobCtlGetProjectStoneJobRspItemWorkMdl> EmployeeProjectStoneJobWorkList { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    [JsonPropertyName("o")]
    public List<MbsWrkJobCtlGetProjectStoneJobRspItemFileMdl> EmployeeProjectStoneJobWorkFileList { get; set; }

}