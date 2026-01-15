using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-角色-取得多筆-回應模型</summary>
public class MbsBscCtlGetManyRoleRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者角色-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyRoleRspItemMdl> ManagerRoleList { get; set; }
}