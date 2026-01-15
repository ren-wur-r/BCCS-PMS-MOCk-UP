using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-取得多筆-回應模型</summary>
public class MbsSysRolCtlGetManyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-角色-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysRolCtlGetManyRspItemMdl> ManagerRoleList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}