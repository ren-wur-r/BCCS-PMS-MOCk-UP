using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工項]-請求模型</summary>
public class CoEmpPsjwDbGetManyFromMbsWrkJobReqMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

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