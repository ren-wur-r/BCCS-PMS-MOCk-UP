using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-展示層-取得多筆產品-請求模型</summary>
public class MbsBscCtlGetManyProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品-主類別ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子類別ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageIndex { get; set; }
}
