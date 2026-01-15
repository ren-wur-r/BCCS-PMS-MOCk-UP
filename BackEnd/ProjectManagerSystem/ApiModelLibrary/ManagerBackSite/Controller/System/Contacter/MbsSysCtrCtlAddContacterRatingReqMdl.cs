using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-新增窗口開發評等-請求模型</summary>
public class MbsSysCtrCtlAddContacterRatingReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口開發評等原因ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    [JsonPropertyName("c")]
    [StringLength(500, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterRatingRemark { get; set; }
}
