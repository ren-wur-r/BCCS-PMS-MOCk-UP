using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得單筆窗口開發評等原因-回應模型</summary>
public class MbsSysCtrCtlGetContacterRatingReasonRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口開發評等原因-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    [JsonPropertyName("b")]
    public bool ManagerContacterRatingReasonStatus { get; set; }
}
