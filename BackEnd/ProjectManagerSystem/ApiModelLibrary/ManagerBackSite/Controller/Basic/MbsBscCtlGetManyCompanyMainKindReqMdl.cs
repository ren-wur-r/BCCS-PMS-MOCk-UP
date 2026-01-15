using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司主分類-請求模型</summary>
public class MbsBscCtlGetManyCompanyMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>公司主分類名稱(模糊查詢)</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("b")]
    public int PageIndex { get; set; }
}
