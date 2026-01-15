using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得公司營業地點-回應模型</summary>
public class MbsBscCtlGetManagerCompanyLocationRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司-ID</summary>
    [JsonPropertyName("b")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司營業地點-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>原子-縣市</summary>
    [JsonPropertyName("d")]
    public DbAtomCityEnum AtomCity { get; set; }

    /// <summary>地址</summary>
    [JsonPropertyName("e")]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>電話</summary>
    [JsonPropertyName("f")]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    [JsonPropertyName("g")]
    public DbAtomManagerCompanyStatusEnum ManagerCompanyLocationStatus { get; set; }
}
