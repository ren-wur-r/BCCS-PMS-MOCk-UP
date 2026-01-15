using System;
using System.Collections.Generic;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-邏輯-更新專案里程碑-請求項目工項模型</summary>
public class MbsWrkPrjLgcUpdateProjectStoneReqItemJobMdl
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

    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    public List<MbsWrkPrjLgcUpdateProjectStoneReqItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

}