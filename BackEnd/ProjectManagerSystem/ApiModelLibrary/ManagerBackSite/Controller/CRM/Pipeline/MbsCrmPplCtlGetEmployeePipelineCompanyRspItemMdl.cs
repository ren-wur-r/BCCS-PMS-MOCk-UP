using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得客戶-項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineCompanyRspItemMdl
{
    /// <summary>公司統一編號</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>客戶公司名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-人員規模</summary>
    [JsonPropertyName("c")]
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }

    /// <summary>原子-客戶分級</summary>
    [JsonPropertyName("d")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>公司主類別名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>公司子類別名稱</summary>
    [JsonPropertyName("f")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>原子-縣市</summary>
    [JsonPropertyName("g")]
    public DbAtomCityEnum? AtomCity { get; set; }

    /// <summary>公司營業地點地址</summary>
    [JsonPropertyName("h")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>公司營業地點電話</summary>
    [JsonPropertyName("i")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>公司營業地點狀態</summary>
    [JsonPropertyName("j")]
    public DbAtomManagerCompanyStatusEnum? ManagerCompanyLocationStatus { get; set; }
}
