using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得活動名單-回應模型</summary>
public class MbsCrmPhnCtlGetActivityEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    #region 基本資訊

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    [JsonPropertyName("a")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    #endregion

    /// <summary>原始客戶資訊</summary>
    [JsonPropertyName("b")]
    public MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalCompanyItemMdl OriginalCompany { get; set; }

    /// <summary>是否有客戶資訊</summary>
    [JsonPropertyName("c")]
    public bool HasCompany { get; set; }

    /// <summary>客戶資訊</summary>
    [JsonPropertyName("d")]
    public MbsCrmPhnCtlGetActivityEmployeePipelineRspCompanyItemMdl Company { get; set; }

    /// <summary>原始窗口資訊</summary>
    [JsonPropertyName("e")]
    public MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalContacterItemMdl OriginalContacter { get; set; }

    /// <summary>窗口資訊列表</summary>
    [JsonPropertyName("f")]
    public List<MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl> ContacterList { get; set; }

    #region 報名狀態

    /// <summary>Teams會議持續時間</summary>
    [JsonPropertyName("g")]
    public string TeamsMeetingDuration { get; set; }

    /// <summary>角色</summary>
    [JsonPropertyName("h")]
    public string TeamsRole { get; set; }

    #endregion

    /// <summary>產品列表</summary>
    [JsonPropertyName("i")]
    public List<MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl> ProductList { get; set; }

    /// <summary>電銷紀錄列表</summary>
    [JsonPropertyName("j")]
    public List<MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl> PhoneList { get; set; }

    /// <summary>指派業務記錄列表</summary>
    [JsonPropertyName("k")]
    public List<MbsCrmPhnCtlGetActivityEmployeePipelineRspSalerItemMdl> SalerList { get; set; }
}
