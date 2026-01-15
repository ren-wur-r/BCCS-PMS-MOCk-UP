using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目工作模型</summary>
public class MbsWrkJobLgcGetProjectStoneJobRspItemWorkMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-ID</summary>
    [JsonPropertyName("d")]
    public int EmployeeID { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    [JsonPropertyName("e")]
    public int EmployeeProjectStoneJobWorkRemark { get; set; }
}