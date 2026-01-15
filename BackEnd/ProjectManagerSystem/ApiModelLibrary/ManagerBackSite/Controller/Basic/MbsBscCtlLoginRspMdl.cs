using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;
public class MbsBscCtlLoginRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>登入令牌</summary>
    [JsonPropertyName("a")]
    public string LoginToken { get; set; }

}
