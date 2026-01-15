using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-更新-請求模型</summary>
public class MbsSysRolCtlUpdateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    [JsonPropertyName("c")]
    public int? ManagerRegionID { get; set; }

    /// <summary>管理者-角色-部門ID</summary>
    [JsonPropertyName("d")]
    public int? ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-備註</summary>
    [JsonPropertyName("e")]
    public string ManagerRoleRemark { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    [JsonPropertyName("f")]
    public bool? ManagerRoleIsEnable { get; set; }

    /// <summary>管理者-角色-權限列表</summary>
    [JsonPropertyName("g")]
    public List<MbsSysRolCtlUpdateReqItemMdl> ManagerRolePermissionList { get; set; }
}