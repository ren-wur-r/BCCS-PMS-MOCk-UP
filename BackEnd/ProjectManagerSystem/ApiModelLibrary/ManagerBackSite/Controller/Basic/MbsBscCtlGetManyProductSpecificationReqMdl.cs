using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-產品規格-取得多筆-請求模型</summary>
public class MbsBscCtlGetManyProductSpecificationReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductID { get; set; }

    /// <summary>產品規格-名稱</summary>
    [JsonPropertyName("b")]
    public string ProductSpecificationName { get; set; }

    /// <summary>是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ProductSpecificationIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageIndex { get; set; }
}