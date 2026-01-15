using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Region;

/// <summary>管理者後台-系統-地區-控制器-取得單筆-請求模型</summary>
public class MbsSysRgnCtlGetReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-地區-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerRegionID { get; set; }
}