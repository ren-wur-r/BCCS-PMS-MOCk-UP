using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Common;

namespace ApiModelLibrary.ManagerBackSite.Controller.Base;

/// <summary>管理者後台-控制器-基底-回應模型</summary>
public class MbsCtlBaseRspMdl
{
    /// <summary>錯誤代碼</summary>
    [Required]
    [JsonPropertyName("aa")]
    public MbsErrorCodeEnum ErrorCode { get; set; }
}

