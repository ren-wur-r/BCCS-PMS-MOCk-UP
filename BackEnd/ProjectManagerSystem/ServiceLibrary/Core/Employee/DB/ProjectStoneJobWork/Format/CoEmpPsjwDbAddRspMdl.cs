using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-新增-回應模型</summary>
public class CoEmpPsjwDbAddRspMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-建立時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkCreateTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkUpdateTime { get; set; }


}
