using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomManagerContacter;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得活動名單-回應模型</summary>
public class MbsCrmActCtlGetActivityEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    #region 基本資訊

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    [JsonPropertyName("a")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    #endregion

    #region 客戶

    /// <summary>公司統一編號</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>客戶公司名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-人員規模</summary>
    [JsonPropertyName("d")]
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }

    /// <summary>原子-客戶分級</summary>
    [JsonPropertyName("e")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>公司主類別名稱</summary>
    [JsonPropertyName("f")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>公司子類別名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>原子-縣市</summary>
    [JsonPropertyName("h")]
    public DbAtomCityEnum? AtomCity { get; set; }

    /// <summary>公司營業地點地址</summary>
    [JsonPropertyName("i")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>公司營業地點電話</summary>
    [JsonPropertyName("j")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>公司營業地點狀態</summary>
    [JsonPropertyName("k")]
    public DbAtomManagerCompanyStatusEnum? ManagerCompanyLocationStatus { get; set; }

    #endregion

    #region 窗口

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("l")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("m")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("n")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("o")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("p")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口電話(市話)</summary>
    [JsonPropertyName("q")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>窗口是否個資同意</summary>
    [JsonPropertyName("r")]
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>窗口在職狀態</summary>
    [JsonPropertyName("s")]
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>窗口開發評等ID</summary>
    [JsonPropertyName("t")]
    public DbAtomManagerContacterRatingKindEnum AtomRatingKind { get; set; }

    #endregion

    #region 報名狀態

    /// <summary>Teams會議持續時間</summary>
    [JsonPropertyName("u")]
    public string TeamsMeetingDuration { get; set; }

    /// <summary>角色</summary>
    [JsonPropertyName("v")]
    public string TeamsRole { get; set; }

    #endregion   
}
