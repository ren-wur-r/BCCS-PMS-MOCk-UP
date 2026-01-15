using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Region;

/// <summary>管理者後台-系統-地區-控制器-更新-請求模型</summary>
public class MbsSysRgnCtlUpdateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-地區-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ManagerRegionIsEnable { get; set; }
}