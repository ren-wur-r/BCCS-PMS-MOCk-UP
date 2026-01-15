using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-取得多筆-請求模型</summary>
public class MbsSysEmpCtlGetManyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-部門-ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? EmployeeIsEnable { get; set; }

    /// <summary>員工-帳號</summary>
    [JsonPropertyName("d")]
    public string EmployeeAccount { get; set; }

    /// <summary>頁面索引</summary>
    [JsonPropertyName("e")]
    [Required]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [JsonPropertyName("f")]
    [Required]
    public int PageSize { get; set; }
}