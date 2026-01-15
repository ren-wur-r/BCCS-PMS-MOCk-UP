using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制-取得員工資訊-回應模型</summary>
public class MbsBscCtlGetEmployeeRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-名稱</summary>
    [JsonPropertyName("a")]
    public string EmployeeName { get; set; }

    /// <summary>管理者後台-基本-控制-取得員工資訊-回應項目模型</summary>
    [JsonPropertyName("b")]
    public List<MbsBscCtlGetEmployeeRspItemMdl> ManagerBackSiteMenuPermissionList { get; set; }

    /// <summary>員工-帳號</summary>
    [JsonPropertyName("c")]
    public string EmployeeAccount { get; set; }

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

    /// <summary>管理者-部門-ID</summary>
    [JsonPropertyName("h")]
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("i")]
    public string ManagerDepartmentName { get; set; }

}