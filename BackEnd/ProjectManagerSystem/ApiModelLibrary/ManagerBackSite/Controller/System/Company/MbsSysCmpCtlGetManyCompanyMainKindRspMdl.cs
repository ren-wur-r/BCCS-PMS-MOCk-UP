using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司主分類-回應模型</summary>
public class MbsSysCmpCtlGetManyCompanyMainKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-公司主分類-清單</summary>
    [JsonPropertyName("a")]
    public List<MbsSysCmpCtlGetManyCompanyMainKindRspItemMdl> ManagerCompanyMainKindList { get; set; }

    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}