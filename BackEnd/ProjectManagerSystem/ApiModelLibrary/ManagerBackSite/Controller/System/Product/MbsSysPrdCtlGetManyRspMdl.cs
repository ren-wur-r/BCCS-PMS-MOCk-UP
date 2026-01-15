using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆-回應模型</summary>
public class MbsSysPrdCtlGetManyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysPrdCtlGetManyRspItemMdl> ManagerProductList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}