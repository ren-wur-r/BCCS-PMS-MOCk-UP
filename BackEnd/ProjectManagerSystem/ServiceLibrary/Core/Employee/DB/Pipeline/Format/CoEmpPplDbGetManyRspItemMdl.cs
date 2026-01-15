using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.EmployeePipeline;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得多筆-回應項目模型</summary>
public class CoEmpPplDbGetManyRspItemMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者活動ID</summary>
    public int? ManagerActivityID { get; set; }

    /// <summary>客戶公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>客戶公司營業地點ID</summary>
    public int? ManagerCompanyLocationID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>商機-業務員工ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>需求確認狀態</summary>
    public DbEmployeePipelineStageCheckEnum? BusinessNeedStatus { get; set; }

    /// <summary>時程確認狀態</summary>
    public DbEmployeePipelineStageCheckEnum? BusinessTimelineStatus { get; set; }

    /// <summary>預算確認狀態</summary>
    public DbEmployeePipelineStageCheckEnum? BusinessBudgetStatus { get; set; }

    /// <summary>簡報確認狀態</summary>
    public DbEmployeePipelineStageCheckEnum? BusinessPresentationStatus { get; set; }

    /// <summary>報價確認狀態</summary>
    public DbEmployeePipelineStageCheckEnum? BusinessQuotationStatus { get; set; }

    /// <summary>議價確認狀態</summary>
    public DbEmployeePipelineStageCheckEnum? BusinessNegotiationStatus { get; set; }

    /// <summary>階段備註</summary>
    public string BusinessStageRemark { get; set; }
}
