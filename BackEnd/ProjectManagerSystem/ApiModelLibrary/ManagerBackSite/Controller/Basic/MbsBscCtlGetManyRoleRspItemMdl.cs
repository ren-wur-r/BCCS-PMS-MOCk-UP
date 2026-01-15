using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-角色-取得多筆-回應項目模型</summary>
public class MbsBscCtlGetManyRoleRspItemMdl
{
    /// <summary>管理者角色-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerRoleID { get; set; }

    /// <summary>管理者角色-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRoleName { get; set; }

    /// <summary>管理者角色-地區ID</summary>
    [JsonPropertyName("c")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者角色-地區名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者角色-部門名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>是否啟用</summary>
    [JsonPropertyName("f")]
    public bool ManagerRoleIsEnable { get; set; }
}