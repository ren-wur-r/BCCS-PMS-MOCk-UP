using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆窗口開發評等原因-回應項目模型</summary>
public class MbsBscCtlGetManyContacterRatingReasonRspItemMdl
{
    /// <summary>窗口開發評等原因ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>窗口開發評等原因名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>窗口開發評等原因狀態</summary>
    [JsonPropertyName("c")]
    public bool ManagerContacterRatingReasonStatus { get; set; }
}
