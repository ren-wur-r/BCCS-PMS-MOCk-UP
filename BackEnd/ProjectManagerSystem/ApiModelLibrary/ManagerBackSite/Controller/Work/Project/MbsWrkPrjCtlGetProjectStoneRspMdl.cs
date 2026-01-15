using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑-回應模型</summary>
public class MbsWrkPrjCtlGetProjectStoneRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-專案里程碑-名稱</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工-專案里程碑-前N日通知</summary>
    [JsonPropertyName("d")]
    public int EmployeeProjectStonePreNotifyDay { get; set; }

    /// <summary>員工-專案里程碑工項-列表</summary>
    [JsonPropertyName("e")]
    public List<MbsWrkPrjCtlGetProjectStoneRspItemJobMdl> EmployeeProjectStoneJobList { get; set; }

}