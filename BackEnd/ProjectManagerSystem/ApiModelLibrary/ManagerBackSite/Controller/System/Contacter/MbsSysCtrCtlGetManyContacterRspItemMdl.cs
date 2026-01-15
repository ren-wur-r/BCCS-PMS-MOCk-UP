using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得多筆窗口-回應項目模型</summary>
public class MbsSysCtrCtlGetManyContacterRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>管理者公司名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyName { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>管理者窗口-電話</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>管理者窗口-部門</summary>
    [JsonPropertyName("f")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>管理者窗口-職稱</summary>
    [JsonPropertyName("g")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>管理者窗口-開發評等</summary>
    [JsonPropertyName("h")]
    public DbAtomManagerContacterRatingKindEnum ManagerContacterRatingKind { get; set; }
}
