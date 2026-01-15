using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增-回應模型</summary>
public class CoEmpPsjeDbAddRspMdl
{
    /// <summary>員工-專案里程碑工項執行者-建立時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobExecutorCreateTime { get; set; }

    /// <summary>員工-專案里程碑工項執行者-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobExecutorUpdateTime { get; set; }
}