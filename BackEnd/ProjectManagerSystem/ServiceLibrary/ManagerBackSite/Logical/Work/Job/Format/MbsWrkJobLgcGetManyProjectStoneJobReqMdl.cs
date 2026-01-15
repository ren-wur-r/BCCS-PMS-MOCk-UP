using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項-請求模型</summary>
public class MbsWrkJobLgcGetManyProjectStoneJobReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>開始-員工-專案里程碑工項-訖止時間</summary>
    public DateTimeOffset? StartEmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>結束-員工-專案里程碑工項-訖止時間</summary>
    public DateTimeOffset? EndEmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>頁數</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }

}