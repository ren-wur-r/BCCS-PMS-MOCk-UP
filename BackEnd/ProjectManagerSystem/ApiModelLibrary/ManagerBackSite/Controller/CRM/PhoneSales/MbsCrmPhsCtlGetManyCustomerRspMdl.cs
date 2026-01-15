using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

public class MbsCrmPhsCtlGetManyCustomerRspMdl : MbsCtlBaseRspMdl
{
    [JsonPropertyName("a")]
    public List<MbsCrmPhsCtlGetManyCustomerRspItemMdl> CustomerList { get; set; }

    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
