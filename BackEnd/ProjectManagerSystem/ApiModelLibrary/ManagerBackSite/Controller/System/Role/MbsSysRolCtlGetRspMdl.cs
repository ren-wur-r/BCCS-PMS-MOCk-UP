using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-取得單筆-回應模型</summary>
public class MbsSysRolCtlGetRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-角色-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    [JsonPropertyName("b")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-角色-地區名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-角色-部門ID</summary>
    [JsonPropertyName("d")]
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-部門名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-角色-備註</summary>
    [JsonPropertyName("f")]
    public string ManagerRoleRemark { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    [JsonPropertyName("g")]
    public bool ManagerRoleIsEnable { get; set; }

    /// <summary>員工-人數</summary>
    [JsonPropertyName("h")]
    public int EmployeeCount { get; set; }

    /// <summary>管理者-角色-權限列表</summary>
    [JsonPropertyName("i")]
    public List<MbsSysRolCtlGetRspItemMdl> ManagerRolePermissionList { get; set; }
}