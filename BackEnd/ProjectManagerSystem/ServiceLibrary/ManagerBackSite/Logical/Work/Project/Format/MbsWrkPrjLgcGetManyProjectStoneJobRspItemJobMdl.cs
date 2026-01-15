using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項-工項-回應模型</summary>
public class MbsWrkPrjLgcGetManyProjectStoneJobRspItemJobMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-工時</summary>
    public int EmployeeProjectStoneJobWorkHour { get; set; }

    /// <summary>原子-員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工-專案里程碑工項-執行者人數</summary>
    public int EmployeeProjectStoneJobExecutorCount { get; set; }

    /// <summary>員工-專案里程碑-工項清單列表</summary>
    public List<MbsWrkPrjLgcGetManyProjectStoneJobRspItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }
}