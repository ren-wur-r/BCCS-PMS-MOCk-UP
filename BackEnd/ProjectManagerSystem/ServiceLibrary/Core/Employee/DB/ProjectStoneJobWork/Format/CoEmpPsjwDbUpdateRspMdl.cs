using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-更新-回應模型</summary>
public class CoEmpPsjwDbUpdateRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkUpdateTime { get; set; }
}
