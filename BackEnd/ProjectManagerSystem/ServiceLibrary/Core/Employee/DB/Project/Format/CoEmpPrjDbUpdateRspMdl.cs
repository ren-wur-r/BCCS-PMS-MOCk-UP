using System;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-更新-回應模型</summary>
public class CoEmpPrjDbUpdateRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工-專案-更新時間</summary>
    public DateTimeOffset EmployeeProjectUpdateTime { get; set; }
}
