using System.Collections.Generic;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.EmployeePipeline;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得名單-回應模型</summary>
public class MbsCrmPplLgcGetEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    #region 基本資訊

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>商機-承辦業務員工ID</summary>
    public int EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機-承辦業務員工名稱</summary>
    public string EmployeePipelineSalerEmployeeName { get; set; }

    /// <summary>商機-承辦業務地區ID</summary>
    public int EmployeePipelineSalerRegionID { get; set; }

    /// <summary>商機-承辦業務地區名稱</summary>
    public string EmployeePipelineSalerRegionName { get; set; }

    /// <summary>商機-承辦業務部門ID</summary>
    public int EmployeePipelineSalerDepartmentID { get; set; }

    /// <summary>商機-承辦業務部門名稱</summary>
    public string EmployeePipelineSalerDepartmentName { get; set; }

    #endregion      

    /// <summary>客戶資訊</summary>
    public MbsCrmPplLgcGetEmployeePipelineRspCompanyItemMdl Company { get; set; }

    /// <summary>窗口資訊列表</summary>
    public List<MbsCrmPplLgcGetEmployeePipelineRspContacterItemMdl> ContacterList { get; set; }

    /// <summary>尚未回覆業務紀錄</summary>
    public MbsCrmPplLgcGetEmployeePipelineRspPendingSalerItemMdl PendingEmployeePipelineSaler { get; set; }

    /// <summary>業務紀錄列表</summary>
    public List<MbsCrmPplLgcGetEmployeePipelineRspSalerItemMdl> EmployeePipelineSalerList { get; set; }

    /// <summary>業務商機開發紀錄列表</summary>
    public List<MbsCrmPplLgcGetEmployeePipelineRspSalerTrackingItemMdl> EmployeePipelineSalerTrackingList { get; set; }

    /// <summary>商機產品列表</summary>
    public List<MbsCrmPplLgcGetEmployeePipelineRspProductItemMdl> EmployeePipelineProductList { get; set; }

    /// <summary>履約期限列表</summary>
    public List<MbsCrmPplLgcGetEmployeePipelineRspDueItemMdl> EmployeePipelineDueList { get; set; }

    /// <summary>發票紀錄列表</summary>
    public List<MbsCrmPplLgcGetEmployeePipelineRspBillItemMdl> EmployeePipelineBillList { get; set; }

    /// <summary>商機階段檢核資料</summary>
    public MbsCrmPplLgcGetEmployeePipelineRspStageStatusMdl StageStatus { get; set; }
}

/// <summary>商機階段檢核資料</summary>
public class MbsCrmPplLgcGetEmployeePipelineRspStageStatusMdl
{
    public DbEmployeePipelineStageCheckEnum? BusinessNeedStatus { get; set; }
    public DbEmployeePipelineStageCheckEnum? BusinessTimelineStatus { get; set; }
    public DbEmployeePipelineStageCheckEnum? BusinessBudgetStatus { get; set; }
    public DbEmployeePipelineStageCheckEnum? BusinessPresentationStatus { get; set; }
    public DbEmployeePipelineStageCheckEnum? BusinessQuotationStatus { get; set; }
    public DbEmployeePipelineStageCheckEnum? BusinessNegotiationStatus { get; set; }
    public string BusinessStageRemark { get; set; }
}
