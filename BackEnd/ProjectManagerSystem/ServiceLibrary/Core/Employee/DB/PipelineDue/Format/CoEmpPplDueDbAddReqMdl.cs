using System;

namespace ServiceLibrary.Core.Employee.DB.PipelineDue.Format;

/// <summary>核心-員工-商機履約期限-資料庫-新增-請求模型</summary>
public class CoEmpPplDueDbAddReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>履約日期</summary>
    public DateTimeOffset EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineDueRemark { get; set; }
}
