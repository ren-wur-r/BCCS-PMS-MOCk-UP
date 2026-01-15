using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlAddCustomerReqContacterItemMdl
{
    [Required]
    [JsonPropertyName("a")]
    public string ContacterName { get; set; }

    [JsonPropertyName("b")]
    public string ContacterJobTitle { get; set; }

    [JsonPropertyName("c")]
    public string ContacterDepartment { get; set; }

    [JsonPropertyName("d")]
    public string ContacterPhone { get; set; }

    [JsonPropertyName("e")]
    public string ContacterMobile { get; set; }

    [Required]
    [EmailAddress]
    [JsonPropertyName("f")]
    public string ContacterEmail { get; set; }

    [Required]
    [JsonPropertyName("g")]
    public bool IsPrimary { get; set; }
}
