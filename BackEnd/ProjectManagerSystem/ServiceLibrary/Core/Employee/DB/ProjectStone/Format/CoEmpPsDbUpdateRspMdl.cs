using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-資料庫-更新-回應模型</summary>
public class CoEmpPsDbUpdateRspMdl
{
    /// <summary>受影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工里程碑-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneUpdateTime { get; set; }
}