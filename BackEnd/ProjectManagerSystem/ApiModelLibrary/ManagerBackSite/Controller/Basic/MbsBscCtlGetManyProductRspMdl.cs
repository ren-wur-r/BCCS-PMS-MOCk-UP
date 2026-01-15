using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-展示層-取得多筆產品-回應模型</summary>
public class MbsBscCtlGetManyProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>產品列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyProductRspItemMdl> ProductList { get; set; }
}
