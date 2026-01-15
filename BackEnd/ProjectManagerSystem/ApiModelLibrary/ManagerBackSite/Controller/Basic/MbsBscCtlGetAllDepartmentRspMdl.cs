using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得全部管理者部門-回應模型</summary>
public class MbsBscCtlGetAllDepartmentRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者部門-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetAllDepartmentRspItemMdl> ManagerDepartmentList { get; set; }
}