using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-客戶名單-客戶窗口-控制器-新增窗口-請求模型-窗口開發評等項目模型</summary>
public class MbsSysCtrCtlAddContacterReqRatingItemMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterRatingRemark { get; set; }
}
