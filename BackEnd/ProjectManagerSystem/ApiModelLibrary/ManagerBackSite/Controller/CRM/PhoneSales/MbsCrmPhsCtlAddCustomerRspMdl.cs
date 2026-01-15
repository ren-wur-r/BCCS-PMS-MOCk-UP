using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlAddCustomerRspMdl : MbsCtlBaseRspMdl
{
    [JsonPropertyName("a")]
    public int PhoneSalesCustomerID { get; set; }
}
