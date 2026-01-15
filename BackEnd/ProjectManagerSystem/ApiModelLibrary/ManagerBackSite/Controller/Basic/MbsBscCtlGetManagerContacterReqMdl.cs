using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得窗口-請求模型</summary>
public class MbsBscCtlGetManagerContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }
}
