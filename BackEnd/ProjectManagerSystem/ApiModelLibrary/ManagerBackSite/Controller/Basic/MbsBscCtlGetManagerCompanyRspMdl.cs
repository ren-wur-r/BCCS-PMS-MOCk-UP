using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得公司-回應模型</summary>
public class MbsBscCtlGetManagerCompanyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-管理者-公司狀態-列舉</summary>
    [JsonPropertyName("d")]
    public DbAtomManagerCompanyStatusEnum AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    [JsonPropertyName("e")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司主分類-名稱</summary>
    [JsonPropertyName("f")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    [JsonPropertyName("g")]
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者公司子分類-名稱</summary>
    [JsonPropertyName("h")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>原子-客戶分級-列舉</summary>
    [JsonPropertyName("i")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>原子-資安責任等級-列舉</summary>
    [JsonPropertyName("j")]
    public DbAtomSecurityGradeEnum? AtomSecurityGrade { get; set; }

    /// <summary>原子-人員規模-列舉</summary>
    [JsonPropertyName("k")]
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }
}
