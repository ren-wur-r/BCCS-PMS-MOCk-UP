using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]-請求模型</summary>
public class CoEmpPplSlrDbGetManyFromMbsCrmPplReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>業務員ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum? EmployeePipelineSalerStatus { get; set; }
}