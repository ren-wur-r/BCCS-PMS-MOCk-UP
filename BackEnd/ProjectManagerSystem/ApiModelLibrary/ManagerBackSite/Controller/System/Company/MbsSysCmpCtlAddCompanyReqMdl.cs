using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using CommonLibrary.CmnDataAnnotation;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司-請求模型</summary>
public class MbsSysCmpCtlAddCompanyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>統一編號</summary>
    [Required]
    [JsonPropertyName("a")]
    [CmnTaiwanCompanyUnifiedNumber]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    [StringLength(300, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    [Required]
    [JsonPropertyName("c")]
    public DbAtomManagerCompanyStatusEnum AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    [Required]
    [JsonPropertyName("d")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    [Required]
    [JsonPropertyName("e")]
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>客戶分級</summary>
    [JsonPropertyName("f")]
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>資安責任等級</summary>
    [JsonPropertyName("g")]
    public DbAtomSecurityGradeEnum? AtomSecurityGrade { get; set; }

    /// <summary>人員規模</summary>
    [JsonPropertyName("h")]
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }

    /// <summary>關係公司列表</summary>
    [JsonPropertyName("i")]
    public List<MbsSysCmpCtlAddCompanyReqAffiliateItemMdl> ManagerCompanyAffiliateList { get; set; }

    /// <summary>公司營業地點列表</summary>
    [JsonPropertyName("j")]
    public List<MbsSysCmpCtlAddCompanyReqLocationItemMdl> ManagerCompanyLocationList { get; set; }
}