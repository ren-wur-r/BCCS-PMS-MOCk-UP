using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.EmployeePipeline;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機狀態-請求模型</summary>
public class MbsCrmPplLgcUpdatePipelineStatusReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

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
