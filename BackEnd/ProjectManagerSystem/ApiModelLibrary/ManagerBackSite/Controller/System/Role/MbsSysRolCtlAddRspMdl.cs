using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Role;

/// <summary>管理者後台-系統-角色-控制器-新增-回應模型</summary>
public class MbsSysRolCtlAddRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-角色ID</summary>
    [JsonPropertyName("a")]
    public int ManagerRoleID { get; set; }
}