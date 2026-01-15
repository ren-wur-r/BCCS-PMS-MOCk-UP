using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-更新多筆-回應模型</summary>
public class CoEmpPsjDbUpdateManyRspMdl
{
    /// <summary>受影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工里程碑工項-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobUpdateTime { get; set; }
}