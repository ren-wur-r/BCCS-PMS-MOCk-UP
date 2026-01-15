using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-更新-請求模型</summary>
public class MbsSysEmpCtlUpdateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int EmployeeID { get; set; }

    /// <summary>員工-密碼</summary>
    [JsonPropertyName("b")]
    public string EmployeePassword { get; set; }

    /// <summary>員工-名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeeName { get; set; }

    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("d")]
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("e")]
    public bool? EmployeeIsEnable { get; set; }

    /// <summary>員工-權限列表</summary>
    [JsonPropertyName("f")]
    public List<MbsSysEmpCtlUpdateReqItemMdl> EmployeePermissionList { get; set; }

}