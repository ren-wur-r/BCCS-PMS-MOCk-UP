using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司-營業地點項目模型</summary>
public class MbsSysCmpCtlAddCompanyReqLocationItemMdl
{
    /// <summary>管理者公司營業地點-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(100, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>原子-縣市</summary>
    [Required]
    [JsonPropertyName("b")]
    public DbAtomCityEnum AtomCity { get; set; }

    /// <summary>管理者公司營業地點-地址</summary>
    [Required]
    [JsonPropertyName("c")]
    [StringLength(100, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>管理者公司營業地點-電話</summary>
    [JsonPropertyName("d")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    [JsonPropertyName("e")]
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }
}
