using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-匯入eDM-項目-回應模型</summary>
public class MbsCrmActCtlImportEdmRspItemMdl
{
    /// <summary>EDM Email</summary>
    [JsonPropertyName("a")]
    public string EdmContacterEmail { get; set; }

    /// <summary>EDM First Name</summary>
    [JsonPropertyName("b")]
    public string EdmFirstName { get; set; }

    /// <summary>EDM Last Name</summary>
    [JsonPropertyName("c")]
    public string EdmLastName { get; set; }

    /// <summary>EDM Telephone</summary>
    [JsonPropertyName("d")]
    public string EdmContacterTelephone { get; set; }

    /// <summary>EDM 行動電話</summary>
    [JsonPropertyName("e")]
    public string EdmContacterPhone { get; set; }

    /// <summary>EDM 公司名稱</summary>
    [JsonPropertyName("f")]
    public string EdmCompanyName { get; set; }

    /// <summary>EDM 職稱</summary>
    [JsonPropertyName("g")]
    public string EdmContacterJobTitle { get; set; }

    /// <summary>EDM 公司電話</summary>
    [JsonPropertyName("h")]
    public string EdmCompanyPhone { get; set; }

    /// <summary>EDM 公司傳真</summary>
    [JsonPropertyName("i")]
    public string EdmCompanyFax { get; set; }

    /// <summary>EDM 公司地址</summary>
    [JsonPropertyName("j")]
    public string EdmCompanyAddress { get; set; }

    /// <summary>EDM 備注</summary>
    [JsonPropertyName("k")]
    public string EdmRemark { get; set; }

    /// <summary>EDM 部門</summary>
    [JsonPropertyName("l")]
    public string EdmContacterDepartment { get; set; }

    /// <summary>EDM 主類別碼</summary>
    [JsonPropertyName("m")]
    public string EdmCompanyMainKind { get; set; }

    /// <summary>EDM 分類碼</summary>
    [JsonPropertyName("n")]
    public string EdmCompanySubKind { get; set; }

    /// <summary>EDM Account Sales</summary>
    [JsonPropertyName("o")]
    public string EdmAccountSales { get; set; }

    /// <summary>EDM 區域</summary>
    [JsonPropertyName("p")]
    public string EdmRegion { get; set; }

    /// <summary>EDM Created Date</summary>
    [JsonPropertyName("q")]
    public string EdmCreatedDate { get; set; }

    /// <summary>EDM device</summary>
    [JsonPropertyName("r")]
    public string EdmDevice { get; set; }
}
