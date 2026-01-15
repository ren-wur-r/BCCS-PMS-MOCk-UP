using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-區域-取得多筆-請求模型</summary>
public class MbsBscCtlGetManyRegionReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>區域-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerRegionName { get; set; }

    /// <summary>是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerRegionIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }
}