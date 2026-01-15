using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-取得多筆-請求模型</summary>
public class MbsSysRolCtlGetManyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-角色-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-角色-是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerRoleIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [JsonPropertyName("c")]
    [Required]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [JsonPropertyName("d")]
    [Required]
    public int PageSize { get; set; }
}