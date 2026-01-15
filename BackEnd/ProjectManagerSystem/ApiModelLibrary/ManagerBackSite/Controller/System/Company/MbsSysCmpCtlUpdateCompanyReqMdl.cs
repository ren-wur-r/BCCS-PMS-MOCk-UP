using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using CommonLibrary.CmnDataAnnotation;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-更新公司-請求模型</summary>
public class MbsSysCmpCtlUpdateCompanyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    [JsonPropertyName("b")]
    [CmnTaiwanCompanyUnifiedNumber]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    [JsonPropertyName("c")]
    [StringLength(300, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    [JsonPropertyName("d")]
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    [JsonPropertyName("e")]
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    [JsonPropertyName("f")]
    public int? ManagerCompanySubKindID { get; set; }

    /// <summary>客戶分級</summary>
    [JsonPropertyName("g")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>資安責任等級</summary>
    [JsonPropertyName("h")]
    public DbAtomSecurityGradeEnum? AtomSecurityGrade { get; set; }

    /// <summary>人員規模</summary>
    [JsonPropertyName("i")]
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }
}