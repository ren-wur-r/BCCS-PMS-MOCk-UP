using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Region;

/// <summary>管理者後台-系統-地區-控制器-取得多筆-回應模型</summary>
public class MbsSysRgnCtlGetManyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-地區列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysRgnCtlGetManyRspItemMdl> ManagerRegionList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}