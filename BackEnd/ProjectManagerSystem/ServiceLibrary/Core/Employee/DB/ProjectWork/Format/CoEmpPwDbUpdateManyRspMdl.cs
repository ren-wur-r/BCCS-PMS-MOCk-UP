using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-更新多筆-回應模型</summary>
public class CoEmpPwDbUpdateManyRspMdl
{
    /// <summary>受影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工專案工作計劃書-更新時間</summary>
    public DateTimeOffset EmployeeProjectWorkUpdateTime { get; set; }
}