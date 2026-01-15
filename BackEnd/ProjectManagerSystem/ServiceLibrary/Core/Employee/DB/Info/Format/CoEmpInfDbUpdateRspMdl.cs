using System;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-更新-回應模型</summary>
public class CoEmpInfDbUpdateRspMdl
{
    /// <summary>受影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工-更新時間</summary>
    public DateTimeOffset EmployeeUpdateTime { get; set; }
}
