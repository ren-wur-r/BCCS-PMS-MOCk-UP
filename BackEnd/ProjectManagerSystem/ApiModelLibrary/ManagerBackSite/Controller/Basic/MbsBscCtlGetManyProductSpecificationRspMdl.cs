using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-產品規格-取得多筆-回應模型</summary>
public class MbsBscCtlGetManyProductSpecificationRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>產品規格列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyProductSpecificationRspItemMdl> ProductSpecificationList { get; set; }
}