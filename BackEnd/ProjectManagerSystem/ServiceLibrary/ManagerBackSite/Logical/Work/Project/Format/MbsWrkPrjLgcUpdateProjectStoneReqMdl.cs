using System;
using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-邏輯-更新專案里程碑-請求模型</summary>
public class MbsWrkPrjLgcUpdateProjectStoneReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    public DateTimeOffset? EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    public DateTimeOffset? EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工-專案里程碑-前N日通知</summary>
    public int? EmployeeProjectStonePreNotifyDay { get; set; }

    /// <summary>員工-專案里程碑工項-列表</summary>
    public List<MbsWrkPrjLgcUpdateProjectStoneReqItemJobMdl> EmployeeProjectStoneJobList { get; set; }

}