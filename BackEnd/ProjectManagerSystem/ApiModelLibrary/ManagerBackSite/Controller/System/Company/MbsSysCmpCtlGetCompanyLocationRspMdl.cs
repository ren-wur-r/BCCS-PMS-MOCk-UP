using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得公司營業地點-回應模型</summary>
public class MbsSysCmpCtlGetCompanyLocationRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>原子-縣市</summary>
    [JsonPropertyName("c")]
    public DbAtomCityEnum? AtomCity { get; set; }

    /// <summary>管理者公司營業地點-地址</summary>
    [JsonPropertyName("d")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>管理者公司營業地點-電話</summary>
    [JsonPropertyName("e")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    [JsonPropertyName("f")]
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }
}
