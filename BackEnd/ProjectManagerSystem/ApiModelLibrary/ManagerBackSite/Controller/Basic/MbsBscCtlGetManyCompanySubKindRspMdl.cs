using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司子分類-回應模型</summary>
public class MbsBscCtlGetManyCompanySubKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>公司子分類列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyCompanySubKindRspItemMdl> ManagerCompanySubKindList { get; set; }
}
