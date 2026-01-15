using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑工項-回應模型</summary>
public class MbsWrkPrjLgcGetProjectStoneJobRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

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

    /// <summary>員工-專案里程碑工項-工時</summary>
    public int EmployeeProjectStoneJobWorkHour { get; set; }

    /// <summary>原子-專案里程碑工項-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項執行者列表</summary>
    public List<MbsWrkPrjLgcGetProjectStoneJobRspItmeExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

    /// <summary>員工-專案里程碑工項清單列表</summary>
    public List<MbsWrkPrjLgcGetProjectStoneJobRspItmeBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }
}