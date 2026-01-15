using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司子分類-請求模型</summary>
public class MbsBscCtlGetManyCompanySubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>公司主分類ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>公司子分類名稱(模糊查詢)</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>公司子分類是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ManagerCompanySubKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageIndex { get; set; }
}
