using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-新增-請求模型</summary>
public class CoEmpPplDbAddReqMdl
{
    /// <summary>活動ID</summary>
    public int? ManagerActivityID { get; set; }

    /// <summary>客戶公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>客戶公司營業地點ID</summary>
    public int? ManagerCompanyLocationID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>商機業務-業務員工ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }
}
