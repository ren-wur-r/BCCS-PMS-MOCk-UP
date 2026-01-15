using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-是否存在-請求模型</summary>
public class CoEmpPplSlrDbExistsReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務-業務員工ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum? EmployeePipelineSalerStatus { get; set; }
}