using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-新增窗口開發評等原因-請求模型</summary>
public class MbsSysCtrCtlAddContacterRatingReasonReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口開發評等原因-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    [Required]
    [JsonPropertyName("b")]
    public bool ManagerContacterRatingReasonStatus { get; set; }
}
