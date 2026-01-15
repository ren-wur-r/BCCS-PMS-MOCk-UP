using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;
using DataModelLibrary.Database.AtomPhoneSalesCustomerValue;
using DataModelLibrary.Database.AtomPhoneSalesDataSource;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlGetCustomerRspMdl : MbsCtlBaseRspMdl
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
    public string CompanyPhone { get; set; }

    [JsonPropertyName("g")]
    public string CompanyAddress { get; set; }

    [JsonPropertyName("h")]
    public string CompanyWebsite { get; set; }

    [JsonPropertyName("i")]
    public DbAtomPhoneSalesCustomerStatusEnum AtomPhoneSalesCustomerStatus { get; set; }

    [JsonPropertyName("j")]
    public DbAtomPhoneSalesCustomerValueEnum AtomPhoneSalesCustomerValue { get; set; }

    [JsonPropertyName("k")]
    public DbAtomPhoneSalesDataSourceEnum AtomPhoneSalesDataSource { get; set; }

    [JsonPropertyName("l")]
    public decimal ValueScore { get; set; }

    [JsonPropertyName("m")]
    public bool IsExistingCustomer { get; set; }

    [JsonPropertyName("n")]
    public int? ExistingManagerCompanyID { get; set; }

    [JsonPropertyName("o")]
    public string ComparisonMethod { get; set; }

    [JsonPropertyName("p")]
    public int ContactCount { get; set; }

    [JsonPropertyName("q")]
    public DateTimeOffset? LastContactTime { get; set; }

    [JsonPropertyName("r")]
    public DateTimeOffset? NextFollowUpTime { get; set; }

    [JsonPropertyName("s")]
    public int CustomOrder { get; set; }

    [JsonPropertyName("t")]
    public string Tags { get; set; }

    [JsonPropertyName("u")]
    public string Remark { get; set; }

    [JsonPropertyName("v")]
    public List<MbsCrmPhsCtlGetCustomerRspContacterItemMdl> ContacterList { get; set; }

    [JsonPropertyName("w")]
    public DateTimeOffset CreateTime { get; set; }

    [JsonPropertyName("x")]
    public DateTimeOffset UpdateTime { get; set; }
}

public class MbsCrmPhsCtlGetCustomerRspContacterItemMdl
{
    [JsonPropertyName("a")]
    public int PhoneSalesContacterID { get; set; }

    [JsonPropertyName("b")]
    public string ContacterName { get; set; }

    [JsonPropertyName("c")]
    public string ContacterJobTitle { get; set; }

    [JsonPropertyName("d")]
    public string ContacterDepartment { get; set; }

    [JsonPropertyName("e")]
    public string ContacterPhone { get; set; }

    [JsonPropertyName("f")]
    public string ContacterMobile { get; set; }

    [JsonPropertyName("g")]
    public string ContacterEmail { get; set; }

    [JsonPropertyName("h")]
    public bool IsPrimary { get; set; }
}
