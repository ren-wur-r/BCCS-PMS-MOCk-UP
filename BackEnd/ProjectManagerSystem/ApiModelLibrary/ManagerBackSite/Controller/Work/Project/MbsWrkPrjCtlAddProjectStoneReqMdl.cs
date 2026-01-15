using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-新增專案里程碑-請求模型</summary>
public class MbsWrkPrjCtlAddProjectStoneReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-前N日通知</summary>
    [Required]
    [JsonPropertyName("c")]
    public int EmployeeProjectStonePreNotifyDay { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    [Required]
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    [Required]
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-列表</summary>
    [JsonPropertyName("f")]
    public List<MbsWrkPrjCtlAddProjectStoneReqItemJobMdl> EmployeeProjectStoneJobList { get; set; }
}