using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得單筆窗口開發評等-回應模型</summary>
public class MbsSysCtrCtlGetContacterRatingRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterRatingRemark { get; set; }
}
