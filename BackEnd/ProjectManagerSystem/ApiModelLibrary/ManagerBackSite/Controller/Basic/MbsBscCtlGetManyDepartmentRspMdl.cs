using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-部門-取得多筆-回應模型</summary>
public class MbsBscCtlGetManyDepartmentRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者部門-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyDepartmentRspItemMdl> ManagerDepartmentList { get; set; }
}