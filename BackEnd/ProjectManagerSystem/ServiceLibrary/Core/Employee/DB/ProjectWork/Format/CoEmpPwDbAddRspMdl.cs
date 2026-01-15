using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-新增-回應模型</summary>
public class CoEmpPwDbAddRspMdl
{
    /// <summary>員工專案工作計劃書ID</summary>
    public int EmployeeProjectWorkID { get; set; }

    /// <summary>員工專案工作計劃書-建立時間</summary>
    public DateTimeOffset EmployeeProjectWorkCreateTime { get; set; }

}