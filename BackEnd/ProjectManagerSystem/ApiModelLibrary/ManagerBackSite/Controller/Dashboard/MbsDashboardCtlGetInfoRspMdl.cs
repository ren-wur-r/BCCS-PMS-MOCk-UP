using System.Collections.Generic;
using ApiModelLibrary.ManagerBackSite.Common;
using CommonLibrary.CmnEnum;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.Dashboard;

/// <summary>管理者後台-儀表板-控制器-取得資訊-回應模型</summary>
public class MbsDashboardCtlGetInfoRspMdl
{
    /// <summary>錯誤代碼</summary>
    public MbsErrorCodeEnum ErrorCode { get; set; }

    /// <summary>總毛利 (公司)</summary>
    public decimal TotalGrossProfit { get; set; }

    /// <summary>個人毛利</summary>
    public decimal PersonalGrossProfit { get; set; }

    /// <summary>個人商機數</summary>
    public int PersonalPipelineCount { get; set; }

    /// <summary>個人專案數</summary>
    public int PersonalProjectCount { get; set; }

    /// <summary>部門專案數</summary>
    public int DepartmentProjectCount { get; set; }

    /// <summary>專案風險統計 (公司)</summary>
    public List<MbsDashboardCtlGetInfoRspRiskItemMdl> ProjectRiskStatistics { get; set; } = new List<MbsDashboardCtlGetInfoRspRiskItemMdl>();

    /// <summary>個人專案風險統計</summary>
    public List<MbsDashboardCtlGetInfoRspRiskItemMdl> PersonalProjectRiskStatistics { get; set; } = new List<MbsDashboardCtlGetInfoRspRiskItemMdl>();

    /// <summary>部門專案風險統計</summary>
    public List<MbsDashboardCtlGetInfoRspRiskItemMdl> DepartmentProjectRiskStatistics { get; set; } = new List<MbsDashboardCtlGetInfoRspRiskItemMdl>();

    /// <summary>商機階段統計 (公司)</summary>
    public List<MbsDashboardCtlGetInfoRspPipelineStageItemMdl> PipelineStageStatistics { get; set; } = new List<MbsDashboardCtlGetInfoRspPipelineStageItemMdl>();

    /// <summary>商機階段統計 (個人)</summary>
    public List<MbsDashboardCtlGetInfoRspPipelineStageItemMdl> PersonalPipelineStageStatistics { get; set; } = new List<MbsDashboardCtlGetInfoRspPipelineStageItemMdl>();

    /// <summary>商機階段明細 (公司)</summary>
    public List<MbsDashboardCtlGetInfoRspPipelineStageDetailMdl> PipelineStageDetails { get; set; } = new List<MbsDashboardCtlGetInfoRspPipelineStageDetailMdl>();

    /// <summary>商機階段明細 (個人)</summary>
    public List<MbsDashboardCtlGetInfoRspPipelineStageDetailMdl> PersonalPipelineStageDetails { get; set; } = new List<MbsDashboardCtlGetInfoRspPipelineStageDetailMdl>();
    
    // 其他統計欄位可後續擴充
}

/// <summary>管理者後台-儀表板-控制器-取得資訊-回應-風險統計項目</summary>
public class MbsDashboardCtlGetInfoRspRiskItemMdl
{
    /// <summary>專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum Status { get; set; }

    /// <summary>數量</summary>
    public int Count { get; set; }
}

/// <summary>商機階段統計項目</summary>
public class MbsDashboardCtlGetInfoRspPipelineStageItemMdl
{
    /// <summary>商機階段</summary>
    public DbAtomPipelineStatusEnum Status { get; set; }

    /// <summary>數量</summary>
    public int Count { get; set; }
}

/// <summary>商機階段明細</summary>
public class MbsDashboardCtlGetInfoRspPipelineStageDetailMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>階段</summary>
    public DbAtomPipelineStatusEnum Status { get; set; }

    /// <summary>客戶名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>負責業務</summary>
    public string OwnerName { get; set; }

    /// <summary>預估金額</summary>
    public decimal EstimatedAmount { get; set; }
}
