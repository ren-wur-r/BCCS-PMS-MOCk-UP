using System;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]-項目-回應模型</summary>
public class CoEmpPplSlrDbGetManyFromMbsCrmPplRspItemMdl
{
    /// <summary>商機業務ID</summary>
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>業務員工ID</summary>
    public int EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>業務員工名稱</summary>
    public string EmployeePipelineSalerEmployeeName { get; set; }

    /// <summary>商機業務-業務回覆時間</summary>
    public DateTimeOffset? EmployeePipelineSalerReplyTime { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>指派員工名稱</summary>
    public string EmployeePipelineSalerCreateEmployeeName { get; set; }

    /// <summary>商機業務-備注</summary>
    public string EmployeePipelineSalerRemark { get; set; }

    /// <summary>商機業務-建立時間(指派時間)</summary>
    public DateTimeOffset EmployeePipelineSalerCreateTime { get; set; }
}