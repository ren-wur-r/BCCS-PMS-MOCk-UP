using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆窗口-回應項目模型</summary>
public class MbsBscCtlGetManyContacterRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-電子郵件</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterEmail { get; set; }
}
