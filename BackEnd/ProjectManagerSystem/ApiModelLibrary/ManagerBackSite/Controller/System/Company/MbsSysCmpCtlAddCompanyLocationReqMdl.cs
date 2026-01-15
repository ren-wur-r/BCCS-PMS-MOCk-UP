using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司營業地點-請求模型</summary>
public class MbsSysCmpCtlAddCompanyLocationReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司營業地點-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    [StringLength(100, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>原子-縣市</summary>
    [Required]
    [JsonPropertyName("c")]
    public DbAtomCityEnum AtomCity { get; set; }

    /// <summary>管理者公司營業地點-地址</summary>
    [Required]
    [JsonPropertyName("d")]
    [StringLength(100, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>管理者公司營業地點-電話</summary>
    [JsonPropertyName("e")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    [JsonPropertyName("f")]
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }
}
