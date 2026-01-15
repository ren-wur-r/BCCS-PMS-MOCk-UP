using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-區域-取得多筆-回應模型</summary>
public class MbsBscCtlGetManyRegionRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者區域-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyRegionRspItemMdl> ManagerRegionList { get; set; }
}