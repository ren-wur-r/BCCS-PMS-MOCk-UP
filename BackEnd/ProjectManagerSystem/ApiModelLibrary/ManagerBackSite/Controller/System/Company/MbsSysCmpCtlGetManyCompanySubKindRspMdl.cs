using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司子分類-回應模型</summary>
public class MbsSysCmpCtlGetManyCompanySubKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>公司子分類列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysCmpCtlGetManyCompanySubKindRspItemMdl> ManagerCompanySubKindList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}