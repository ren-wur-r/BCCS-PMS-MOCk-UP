using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆窗口開發評等原因-請求模型</summary>
public class MbsBscCtlGetManyContacterRatingReasonReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>窗口開發評等原因名稱(模糊查詢)</summary>
    [JsonPropertyName("a")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>窗口開發評等原因狀態</summary>
    [JsonPropertyName("b")]
    public bool? ManagerContacterRatingReasonStatus { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }
}
