using System;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-更新-請求</summary>
public class CoEmpPplSlrDbUpdateReqMdl
{
    /// <summary>商機業務ID</summary>
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>業務員工ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機業務-業務回覆時間</summary>
    public DateTimeOffset? EmployeePipelineSalerReplyTime { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum? EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-備注</summary>
    public string EmployeePipelineSalerRemark { get; set; }
}
