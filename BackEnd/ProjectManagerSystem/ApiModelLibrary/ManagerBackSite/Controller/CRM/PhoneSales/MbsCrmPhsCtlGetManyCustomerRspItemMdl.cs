using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;
using DataModelLibrary.Database.AtomPhoneSalesCustomerValue;
using DataModelLibrary.Database.AtomPhoneSalesDataSource;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlGetManyCustomerRspItemMdl
{
    [JsonPropertyName("a")]
    public int PhoneSalesCustomerID { get; set; }

    [JsonPropertyName("b")]
    public string CompanyName { get; set; }

    [JsonPropertyName("c")]
    public string UnifiedNumber { get; set; }

    [JsonPropertyName("d")]
    public string Industry { get; set; }

    [JsonPropertyName("e")]
    public string CompanyScale { get; set; }

    [JsonPropertyName("f")]
    public DbAtomPhoneSalesCustomerStatusEnum AtomPhoneSalesCustomerStatus { get; set; }

    [JsonPropertyName("g")]
    public DbAtomPhoneSalesCustomerValueEnum AtomPhoneSalesCustomerValue { get; set; }

    [JsonPropertyName("h")]
    public DbAtomPhoneSalesDataSourceEnum AtomPhoneSalesDataSource { get; set; }

    [JsonPropertyName("i")]
    public bool IsExistingCustomer { get; set; }

    [JsonPropertyName("j")]
    public int ContactCount { get; set; }

    [JsonPropertyName("k")]
    public DateTimeOffset? LastContactTime { get; set; }

    [JsonPropertyName("l")]
    public string PrimaryContacterName { get; set; }

    [JsonPropertyName("m")]
    public string PrimaryContacterEmail { get; set; }

    [JsonPropertyName("n")]
    public string PrimaryContacterPhone { get; set; }

    [JsonPropertyName("o")]
    public int CustomOrder { get; set; }

    [JsonPropertyName("p")]
    public DateTimeOffset? UpdateTime { get; set; }
}
