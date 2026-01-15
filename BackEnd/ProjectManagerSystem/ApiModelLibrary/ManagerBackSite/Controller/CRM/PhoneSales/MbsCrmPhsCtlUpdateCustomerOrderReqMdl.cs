using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlUpdateCustomerOrderReqMdl : MbsCtlBaseReqMdl
{
    [Required]
    [JsonPropertyName("a")]
    public List<MbsCrmPhsCtlUpdateCustomerOrderReqItemMdl> CustomerOrderList { get; set; }
}
