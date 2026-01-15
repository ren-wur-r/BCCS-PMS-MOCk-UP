using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-新增窗口開發評等原因-回應模型</summary>
public class MbsSysCtrCtlAddContacterRatingReasonRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterRatingReasonID { get; set; }
}
