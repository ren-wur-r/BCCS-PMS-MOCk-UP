using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-產品主分類-取得多筆-回應模型</summary>
public class MbsBscCtlGetManyProductMainKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>產品主分類-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyProductMainKindRspItemMdl> ProductMainKindList { get; set; }
}