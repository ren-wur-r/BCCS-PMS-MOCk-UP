using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得多筆窗口開發評等原因-請求模型</summary>
public class MbsSysCtrCtlGetManyContacterRatingReasonReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口開發評等原因-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    [JsonPropertyName("b")]
    public bool? ManagerContacterRatingReasonStatus { get; set; }

    /// <summary>頁碼</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageSize { get; set; }
}
