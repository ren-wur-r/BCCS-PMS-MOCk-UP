using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-取得多筆-回應項目模型</summary>
public class MbsSysRolCtlGetManyRspItemMdl
{
    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRoleName { get; set; }

    /// <summary>員工-數量</summary>
    [JsonPropertyName("c")]
    public int EmployeeCount { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    [JsonPropertyName("d")]
    public bool ManagerRoleIsEnable { get; set; }

}