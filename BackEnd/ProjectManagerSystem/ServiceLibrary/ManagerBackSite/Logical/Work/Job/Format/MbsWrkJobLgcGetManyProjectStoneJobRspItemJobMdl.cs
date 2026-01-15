using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項-回應項目工項模型</summary>
public class MbsWrkJobLgcGetManyProjectStoneJobRspItemJobMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案-名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-執行者-列表</summary>
    public List<MbsWrkJobLgcGetManyProjectStoneJobRspItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

}