using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆角色權限從[角色ID]-請求模型</summary>
public class MbsBscCtlGetManyRolePermissionFromRoleIdReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>角色-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerRoleID { get; set; }
}