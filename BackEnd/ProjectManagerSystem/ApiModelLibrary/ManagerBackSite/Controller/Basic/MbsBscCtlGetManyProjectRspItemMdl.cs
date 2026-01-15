using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆員工專案-回應項目模型</summary>
public class MbsBscCtlGetManyProjectRspItemMdl
{
    /// <summary>員工專案-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectName { get; set; }
}