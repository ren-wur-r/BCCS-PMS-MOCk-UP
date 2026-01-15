using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-部門-取得多筆-回應項目模型</summary>
public class MbsBscCtlGetManyDepartmentRspItemMdl
{
    /// <summary>管理者部門-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者部門-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者部門-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerDepartmentIsEnable { get; set; }
}