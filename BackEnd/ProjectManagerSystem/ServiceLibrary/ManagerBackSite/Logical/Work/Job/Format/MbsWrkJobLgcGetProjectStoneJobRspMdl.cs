using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項-回應模型</summary>
public class MbsWrkJobLgcGetProjectStoneJobRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-專案-名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案-開始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工-專案-結束時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }
    
    /// <summary>員工-專案里程碑工項-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-專案里程碑工項-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    public List<MbsWrkJobLgcGetProjectStoneJobRspItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    public List<MbsWrkJobLgcGetProjectStoneJobRspItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

    /// <summary>員工-專案里程碑工項工作-列表</summary>
    public List<MbsWrkJobLgcGetProjectStoneJobRspItemWorkMdl> EmployeeProjectStoneJobWorkList { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    public List<MbsWrkJobLgcGetProjectStoneJobRspItemFileMdl> EmployeeProjectStoneJobWorkFileList { get; set; }

}