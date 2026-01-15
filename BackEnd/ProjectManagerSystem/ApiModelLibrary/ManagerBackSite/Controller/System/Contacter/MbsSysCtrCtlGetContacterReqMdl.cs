using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得單筆窗口-請求模型</summary>
public class MbsSysCtrCtlGetContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }
}
