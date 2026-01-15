using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得多筆窗口開發評等-回應項目模型</summary>
public class MbsSysCtrCtlGetManyContacterRatingRspItemMdl
{
    /// <summary>管理者窗口開發評等ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterRatingID { get; set; }

    /// <summary>窗口開發評等原因名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterRatingRemark { get; set; }
}
