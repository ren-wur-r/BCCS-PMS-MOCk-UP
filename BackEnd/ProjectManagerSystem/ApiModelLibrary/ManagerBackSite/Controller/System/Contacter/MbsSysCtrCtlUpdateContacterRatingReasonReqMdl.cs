using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-更新窗口開發評等原因-請求模型</summary>
public class MbsSysCtrCtlUpdateContacterRatingReasonReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等原因-名稱</summary>
    [JsonPropertyName("b")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    [JsonPropertyName("c")]
    public bool? ManagerContacterRatingReasonStatus { get; set; }
}
