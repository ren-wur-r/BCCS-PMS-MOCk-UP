using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Department;

/// <summary>管理者後台-系統-部門-控制器-取得多筆-回應項目模型</summary>
public class MbsSysDptCtlGetManyRspItemMdl
{
    /// <summary>管理者-部門-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-部門-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerDepartmentIsEnable { get; set; }
}