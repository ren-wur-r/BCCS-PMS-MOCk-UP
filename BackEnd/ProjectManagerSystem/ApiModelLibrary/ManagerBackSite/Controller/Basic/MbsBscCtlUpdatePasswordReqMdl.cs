using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-修改密碼-請求模型</summary>
public class MbsBscCtlUpdatePasswordReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-舊密碼</summary>
    [Required]
    [JsonPropertyName("a")]
    public string OldPassword { get; set; }

    /// <summary>管理者-新密碼</summary>
    [Required]
    [JsonPropertyName("b")]
    public string NewPassword { get; set; }
}