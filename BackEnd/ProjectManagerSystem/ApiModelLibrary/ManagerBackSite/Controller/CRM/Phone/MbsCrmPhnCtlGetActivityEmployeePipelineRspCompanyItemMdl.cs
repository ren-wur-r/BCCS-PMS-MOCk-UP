using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得活動名單-客戶項目-回應模型</summary>
public class MbsCrmPhnCtlGetActivityEmployeePipelineRspCompanyItemMdl
{
    /// <summary>公司統一編號</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>客戶公司ID</summary>
    [JsonPropertyName("b")]
    public int ManagerCompanyID { get; set; }

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

    /// <summary>公司營業地點ID</summary>
    [JsonPropertyName("i")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>公司營業地點地址</summary>
    [JsonPropertyName("j")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>公司營業地點電話</summary>
    [JsonPropertyName("k")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>公司營業地點狀態</summary>
    [JsonPropertyName("l")]
    public DbAtomManagerCompanyStatusEnum? ManagerCompanyLocationStatus { get; set; }
}
