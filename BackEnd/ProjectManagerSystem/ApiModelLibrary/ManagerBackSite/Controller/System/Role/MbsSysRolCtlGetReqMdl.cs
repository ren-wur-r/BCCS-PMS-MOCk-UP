using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-取得單筆-請求模型</summary>
public class MbsSysRolCtlGetReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerRoleID { get; set; }
}