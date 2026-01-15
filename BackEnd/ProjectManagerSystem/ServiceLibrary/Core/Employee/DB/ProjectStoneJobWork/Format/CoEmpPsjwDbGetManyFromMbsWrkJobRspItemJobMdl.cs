using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工項]-回應項目工項模型</summary>
public class CoEmpPsjwDbGetManyFromMbsWrkJobRspItemJobMdl
{
    /// <summary>員工-專案-ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum EmployeeProjectStoneJobStatus { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工-列表</summary>
    public List<CoEmpPsjDbGetManyRspItemEmployeeMdl> EmployeeList { get; set; }

}