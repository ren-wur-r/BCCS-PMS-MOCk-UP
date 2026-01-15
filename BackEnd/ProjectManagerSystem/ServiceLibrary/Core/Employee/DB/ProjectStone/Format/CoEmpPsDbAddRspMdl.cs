using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-資料庫-新增-回應模型</summary>
public class CoEmpPsDbAddRspMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑-建立時間</summary>
    public DateTimeOffset EmployeeProjectStoneCreateTime { get; set; }

    /// <summary>員工-專案里程碑-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneUpdateTime { get; set; }

}