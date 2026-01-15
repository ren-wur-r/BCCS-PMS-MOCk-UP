using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-產品子分類-取得多筆-請求模型</summary>
public class MbsBscCtlGetManyProductSubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品子分類-主分類ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>產品子分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ProductSubKindName { get; set; }

    /// <summary>是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ProductSubKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageIndex { get; set; }
}