using System;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項工作-回應項目模型</summary>
public class MbsWrkJobLgcGetManyProjectStoneJobWorkRspItemMdl
{
    /// <summary>員工-專案-ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工-專案-名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-姓名</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項工作-ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

}