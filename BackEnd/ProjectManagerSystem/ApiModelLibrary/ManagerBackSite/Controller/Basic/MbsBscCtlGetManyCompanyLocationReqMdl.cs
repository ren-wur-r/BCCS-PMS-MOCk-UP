using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-展示層-取得多筆公司營業地點-請求模型</summary>
public class MbsBscCtlGetManyCompanyLocationReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司營業地點名稱(模糊搜尋)</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }
}
