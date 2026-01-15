using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlUpdateCustomerReqMdl : MbsCtlBaseReqMdl
{
    [Required]
    [JsonPropertyName("a")]
    public int PhoneSalesCustomerID { get; set; }

    [Required]
    [JsonPropertyName("b")]
    public string CompanyName { get; set; }

    [JsonPropertyName("c")]
    public string UnifiedNumber { get; set; }

    [Required]
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

    [Required]
    [JsonPropertyName("i")]
    public DbAtomPhoneSalesCustomerStatusEnum AtomPhoneSalesCustomerStatus { get; set; }

    [JsonPropertyName("j")]
    public string Tags { get; set; }

    [JsonPropertyName("k")]
    public string Remark { get; set; }

    [Required]
    [JsonPropertyName("l")]
    public List<MbsCrmPhsCtlAddCustomerReqContacterItemMdl> ContacterList { get; set; }
}
