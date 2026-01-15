using System;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-新增-請求</summary>
public class CoEmpPplSlrDbAddReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>業務員工ID</summary>
    public int EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機業務-業務回覆時間</summary>
    public DateTimeOffset? EmployeePipelineSalerSalerReplyTime { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>指派員工ID</summary>
    public int CreateEmployeeID { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineSalerRemark { get; set; }
}
