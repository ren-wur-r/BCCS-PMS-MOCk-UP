using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆角色權限從[角色ID]-回應模型</summary>
public class MbsBscCtlGetManyRolePermissionFromRoleIdRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>角色權限-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyRolePermissionFromRoleIdRspItemMdl> RolePermissionList { get; set; }
}