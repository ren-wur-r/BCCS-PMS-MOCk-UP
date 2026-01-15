using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlUpdateCustomerOrderReqItemMdl
{
    [Required]
    [JsonPropertyName("a")]
    public int PhoneSalesCustomerID { get; set; }

    [Required]
    [JsonPropertyName("b")]
    public int CustomOrder { get; set; }
}
