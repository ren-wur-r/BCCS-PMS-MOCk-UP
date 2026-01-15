using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPhoneSalesDataSource;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlAddCustomerReqMdl : MbsCtlBaseReqMdl
{
    [Required]
    [JsonPropertyName("a")]
    public string CompanyName { get; set; }

    [JsonPropertyName("b")]
    public string UnifiedNumber { get; set; }

    [Required]
    [JsonPropertyName("c")]
    public string Industry { get; set; }

    [JsonPropertyName("d")]
    public string CompanyScale { get; set; }

    [JsonPropertyName("e")]
    public string CompanyPhone { get; set; }

    [JsonPropertyName("f")]
    public string CompanyAddress { get; set; }

    [JsonPropertyName("g")]
    public string CompanyWebsite { get; set; }

    [JsonPropertyName("h")]
    public int? ManagerActivityID { get; set; }

    [Required]
    [JsonPropertyName("i")]
    public DbAtomPhoneSalesDataSourceEnum AtomPhoneSalesDataSource { get; set; }

    [JsonPropertyName("j")]
    public string Tags { get; set; }

    [JsonPropertyName("k")]
    public string Remark { get; set; }

    [Required]
    [JsonPropertyName("l")]
    public List<MbsCrmPhsCtlAddCustomerReqContacterItemMdl> ContacterList { get; set; }
}
