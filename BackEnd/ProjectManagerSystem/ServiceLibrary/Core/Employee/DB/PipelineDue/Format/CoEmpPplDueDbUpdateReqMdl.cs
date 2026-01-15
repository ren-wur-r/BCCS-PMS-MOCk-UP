using System;

namespace ServiceLibrary.Core.Employee.DB.PipelineDue.Format;

/// <summary>核心-員工-商機履約期限-資料庫-更新-請求模型</summary>
public class CoEmpPplDueDbUpdateReqMdl
{
    /// <summary>商機履約通知ID</summary>
    public int EmployeePipelineDueID { get; set; }

    /// <summary>履約日期</summary>
    public DateTimeOffset? EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineDueRemark { get; set; }
}
