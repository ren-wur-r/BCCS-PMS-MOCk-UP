using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作-回應項目模型</summary>
public class MbsWrkJobCtlGetManyProjectStoneJobWorkRspItemMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案-名稱</summary>
    [JsonPropertyName("d")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    [JsonPropertyName("e")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    [JsonPropertyName("f")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-姓名</summary>
    [JsonPropertyName("g")]
    public string EmployeeName { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    [JsonPropertyName("h")]
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

}