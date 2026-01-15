using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.EmployeePipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    #region 基本資訊

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    [JsonPropertyName("a")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>商機-承辦業務員工ID</summary>
    [JsonPropertyName("b")]
    public int EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機-承辦業務員工名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeePipelineSalerEmployeeName { get; set; }

    /// <summary>商機-承辦業務地區ID</summary>
    [JsonPropertyName("d")]
    public int EmployeePipelineSalerRegionID { get; set; }

    /// <summary>商機-承辦業務地區名稱</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelineSalerRegionName { get; set; }

    /// <summary>商機-承辦業務部門ID</summary>
    [JsonPropertyName("f")]
    public int EmployeePipelineSalerDepartmentID { get; set; }

    /// <summary>商機-承辦業務部門名稱</summary>
    [JsonPropertyName("g")]
    public string EmployeePipelineSalerDepartmentName { get; set; }

    #endregion

    /// <summary>客戶資訊</summary>
    [JsonPropertyName("h")]
    public MbsCrmPplCtlGetEmployeePipelineRspCompanyItemMdl Company { get; set; }

    /// <summary>窗口資訊列表</summary>
    [JsonPropertyName("i")]
    public List<MbsCrmPplCtlGetEmployeePipelineRspContacterItemMdl> ContacterList { get; set; }

    /// <summary>尚未回覆業務紀錄</summary>
    [JsonPropertyName("j")]
    public MbsCrmPplCtlGetEmployeePipelineRspPendingSalerItemMdl PendingEmployeePipelineSaler { get; set; }

    /// <summary>業務紀錄列表</summary>
    [JsonPropertyName("k")]
    public List<MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl> EmployeePipelineSalerList { get; set; }

    /// <summary>業務商機開發紀錄列表</summary>
    [JsonPropertyName("l")]
    public List<MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl> EmployeePipelineSalerTrackingList { get; set; }

    /// <summary>商機產品列表</summary>
    [JsonPropertyName("m")]
    public List<MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl> EmployeePipelineProductList { get; set; }

    /// <summary>履約期限列表</summary>
    [JsonPropertyName("n")]
    public List<MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl> EmployeePipelineDueList { get; set; }

    /// <summary>發票紀錄列表</summary>
    [JsonPropertyName("o")]
    public List<MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl> EmployeePipelineBillList { get; set; }

    /// <summary>階段檢核資料</summary>
    [JsonPropertyName("p")]
    public MbsCrmPplCtlGetEmployeePipelineRspStageStatusMdl StageStatus { get; set; }
}

/// <summary>商機階段檢核資料</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspStageStatusMdl
{
    [JsonPropertyName("a")]
    public DbEmployeePipelineStageCheckEnum? BusinessNeedStatus { get; set; }

    [JsonPropertyName("b")]
    public DbEmployeePipelineStageCheckEnum? BusinessTimelineStatus { get; set; }

    [JsonPropertyName("c")]
    public DbEmployeePipelineStageCheckEnum? BusinessBudgetStatus { get; set; }

    [JsonPropertyName("d")]
    public DbEmployeePipelineStageCheckEnum? BusinessPresentationStatus { get; set; }

    [JsonPropertyName("e")]
    public DbEmployeePipelineStageCheckEnum? BusinessQuotationStatus { get; set; }

    [JsonPropertyName("f")]
    public DbEmployeePipelineStageCheckEnum? BusinessNegotiationStatus { get; set; }

    [JsonPropertyName("g")]
    public string BusinessStageRemark { get; set; }
}
