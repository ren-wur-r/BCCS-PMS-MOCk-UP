using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-新增-回應模型</summary>
public class CoEmpPsjDbAddRspMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-建立時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobCreateTime { get; set; }

    /// <summary>員工-專案里程碑工項-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobUpdateTime { get; set; }

}