using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆員工資訊-請求模型</summary>
public class MbsBscCtlGetManyEmployeeInfoReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? EmployeeIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }

    /// <summary>管理者-地區-ID</summary>
    [JsonPropertyName("d")]
    public int? ManagerRegionID { get; set; }

    /// <summary>管理者-部門-ID</summary>
    [JsonPropertyName("e")]
    public int? ManagerDepartmentID { get; set; }

    /// <summary>員工-姓名(模糊查詢)</summary>
    [JsonPropertyName("f")]
    public string EmployeeName { get; set; }
}
