using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-新增-請求模型</summary>
public class MbsSysEmpCtlAddReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-帳號</summary>
    [JsonPropertyName("a")]
    [Required]
    public string EmployeeAccount { get; set; }

    /// <summary>員工-密碼</summary>
    [JsonPropertyName("b")]
    [Required]
    public string EmployeePassword { get; set; }

    /// <summary>員工-姓名</summary>
    [JsonPropertyName("c")]
    [Required]
    public string EmployeeName { get; set; }

    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("d")]
    [Required]
    public int ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("e")]
    [Required]
    public bool EmployeeIsEnable { get; set; }

    /// <summary>員工-權限列表</summary>
    [JsonPropertyName("f")]
    public List<MbsSysEmpCtlAddReqItemMdl> EmployeePermissionList { get; set; }
}