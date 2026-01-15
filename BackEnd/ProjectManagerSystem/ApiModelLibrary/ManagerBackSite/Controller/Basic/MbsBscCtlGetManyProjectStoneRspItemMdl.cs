using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-專案里程碑-取得多筆-回應項目模型</summary>
public class MbsBscCtlGetManyProjectStoneRspItemMdl
{
    /// <summary>員工專案里程碑-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneName { get; set; }
}