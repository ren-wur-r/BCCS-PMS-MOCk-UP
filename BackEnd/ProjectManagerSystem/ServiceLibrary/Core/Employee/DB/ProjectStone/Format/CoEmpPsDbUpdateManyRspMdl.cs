using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案石-資料庫-更新多筆-回應模型</summary>
public class CoEmpPsDbUpdateManyRspMdl
{
    /// <summary>受影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工里程碑-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneUpdateTime { get; set; }
}