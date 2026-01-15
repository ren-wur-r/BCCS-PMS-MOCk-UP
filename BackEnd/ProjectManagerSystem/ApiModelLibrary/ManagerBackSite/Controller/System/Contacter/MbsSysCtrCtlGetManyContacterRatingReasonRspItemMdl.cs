using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得多筆窗口開發評等原因-回應項目模型</summary>
public class MbsSysCtrCtlGetManyContacterRatingReasonRspItemMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等原因-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    [JsonPropertyName("c")]
    public bool ManagerContacterRatingReasonStatus { get; set; }
}
