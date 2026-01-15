using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得公司-請求模型</summary>
public class MbsBscCtlGetManagerCompanyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }
}
