using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-更新專案里程碑-請求項目工項模型</summary>
public class MbsWrkPrjCtlUpdateProjectStoneReqItemJobMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    [Required]
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    [Required]
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-工時</summary>
    [Required]
    [JsonPropertyName("e")]
    public int EmployeeProjectStoneJobWorkHour { get; set; }

    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    [Required]
    [JsonPropertyName("f")]
    public List<MbsWrkPrjCtlUpdateProjectStoneReqItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }
}