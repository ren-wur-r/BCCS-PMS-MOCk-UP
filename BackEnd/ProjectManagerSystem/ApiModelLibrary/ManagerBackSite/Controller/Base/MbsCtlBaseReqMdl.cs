using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Base;

/// <summary>員工-控制器-基底-請求模型</summary>
public class MbsCtlBaseReqMdl
{
    /// <summary>員工登入令牌</summary>
    [Required]
    [JsonPropertyName("aa")]
    public string EmployeeLoginToken { get; set; }
}
