using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司主分類-回應模型</summary>
public class MbsBscCtlGetManyCompanyMainKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>公司主分類列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyCompanyMainKindRspItemMdl> ManagerCompanyMainKindList { get; set; }
}
