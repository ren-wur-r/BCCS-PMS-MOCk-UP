using System;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-新增-回應模型</summary>
public class CoEmpInfDbAddRspMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工-建立時間</summary>
    public DateTimeOffset EmployeeCreateTime { get; set; }

    /// <summary>員工-更新時間</summary>
    public DateTimeOffset EmployeeUpdateTime { get; set; }
}
