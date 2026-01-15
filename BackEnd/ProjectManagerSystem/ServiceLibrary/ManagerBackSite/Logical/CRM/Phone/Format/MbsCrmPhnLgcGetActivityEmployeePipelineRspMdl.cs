using System.Collections.Generic;
using DataModelLibrary.Database.AtomPipeline;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得活動名單-回應模型</summary>
public class MbsCrmPhnLgcGetActivityEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    #region 基本資訊

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    #endregion

    /// <summary>原始客戶資訊</summary>
    public MbsCrmPhnLgcGetActivityEmployeePipelineRspOriginalCompanyItemMdl OriginalCompany { get; set; }

    /// <summary>是否有客戶資訊</summary>
    public bool HasCompany { get; set; }

    /// <summary>客戶資訊</summary>
    public MbsCrmPhnLgcGetActivityEmployeePipelineRspCompanyItemMdl Company { get; set; }

    /// <summary>原始窗口資訊</summary>
    public MbsCrmPhnLgcGetActivityEmployeePipelineRspOriginalContacterItemMdl OriginalContacter { get; set; }

    /// <summary>窗口資訊列表</summary>
    public List<MbsCrmPhnLgcGetActivityEmployeePipelineRspContacterItemMdl> ContacterList { get; set; }

    #region 報名狀態

    /// <summary>Teams會議持續時間</summary>
    public string TeamsMeetingDuration { get; set; }

    /// <summary>角色</summary>
    public string TeamsRole { get; set; }

    #endregion

    /// <summary>產品列表</summary>
    public List<MbsCrmPhnLgcGetActivityEmployeePipelineRspProductItemMdl> ProductList { get; set; }

    /// <summary>電銷紀錄列表</summary>
    public List<MbsCrmPhnLgcGetActivityEmployeePipelineRspPhoneItemMdl> PhoneList { get; set; }

    /// <summary>指派業務記錄列表</summary>
    public List<MbsCrmPhnLgcGetActivityEmployeePipelineRspSalerItemMdl> SalerList { get; set; }
}