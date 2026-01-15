using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項-里程碑項目-回應模型</summary>
public class MbsWrkPrjLgcGetManyProjectStoneJobRspItemStoneMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-前N日通知</summary>
    public int EmployeeProjectStonePreNotifyDay { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>原子員工專案-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工-專案里程碑-工項列表</summary>
    public List<MbsWrkPrjLgcGetManyProjectStoneJobRspItemJobMdl> EmployeeProjectStoneJobList { get; set; }
}