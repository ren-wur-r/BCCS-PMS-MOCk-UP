using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-取得單筆-回應模型</summary>
public class MbsSysEmpCtlGetRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-帳號</summary>
    [JsonPropertyName("a")]
    public string EmployeeAccount { get; set; }

    /// <summary>員工-信箱</summary>
    [JsonPropertyName("b")]
    public string EmployeeEmail { get; set; }

    /// <summary>員工-姓名</summary>
    [JsonPropertyName("c")]
    public string EmployeeName { get; set; }

    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("d")]
    public int ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-地區-ID</summary>
    [JsonPropertyName("f")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("h")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("i")]
    public bool EmployeeIsEnable { get; set; }

    /// <summary>員工-權限列表</summary>
    [JsonPropertyName("j")]
    public List<MbsSysEmpCtlGetRspItemMdl> EmployeePermissionList { get; set; }
}