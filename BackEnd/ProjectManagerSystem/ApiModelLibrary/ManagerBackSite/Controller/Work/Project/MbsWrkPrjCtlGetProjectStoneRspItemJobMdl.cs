using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑-回應項目工項模型</summary>
public class MbsWrkPrjCtlGetProjectStoneRspItemJobMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-工時</summary>
    [JsonPropertyName("e")]
    public int EmployeeProjectStoneJobWorkHour { get; set; }

    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    [JsonPropertyName("f")]
    public List<MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }
}