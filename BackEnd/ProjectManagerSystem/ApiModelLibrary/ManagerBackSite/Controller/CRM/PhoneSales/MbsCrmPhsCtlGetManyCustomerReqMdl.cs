using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;
using DataModelLibrary.Database.AtomPhoneSalesCustomerValue;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlGetManyCustomerReqMdl : MbsCtlBaseReqMdl
{
    [JsonPropertyName("a")]
    public DbAtomPhoneSalesCustomerStatusEnum? AtomPhoneSalesCustomerStatus { get; set; }

    [JsonPropertyName("b")]
    public DbAtomPhoneSalesCustomerValueEnum? AtomPhoneSalesCustomerValue { get; set; }

    [JsonPropertyName("c")]
    public string CompanyName { get; set; }

    [JsonPropertyName("d")]
    public string Industry { get; set; }

    [JsonPropertyName("e")]
    public string ContacterName { get; set; }

    [JsonPropertyName("f")]
    public string ContacterEmail { get; set; }

    [JsonPropertyName("g")]
    public bool? IsExistingCustomer { get; set; }

    [Required]
    [JsonPropertyName("h")]
    public int PageIndex { get; set; }

    [Required]
    [JsonPropertyName("i")]
    public int PageSize { get; set; }
}
