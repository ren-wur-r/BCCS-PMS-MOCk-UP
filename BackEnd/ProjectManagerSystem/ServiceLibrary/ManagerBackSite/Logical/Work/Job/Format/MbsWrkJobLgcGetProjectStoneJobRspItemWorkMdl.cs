using System;
using System.Text.Json.Serialization;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項-回應項目工作模型</summary>
public class MbsWrkJobLgcGetProjectStoneJobRspItemWorkMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

    /// <summary>員工-姓名</summary>
    public string EmployeeName { get; set; }

}