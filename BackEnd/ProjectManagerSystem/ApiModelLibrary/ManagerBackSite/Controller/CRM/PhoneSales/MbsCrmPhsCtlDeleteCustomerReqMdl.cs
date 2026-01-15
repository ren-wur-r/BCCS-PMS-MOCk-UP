using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlDeleteCustomerReqMdl : MbsCtlBaseReqMdl
{
    [Required]
    [JsonPropertyName("a")]
    public int PhoneSalesCustomerID { get; set; }
}
