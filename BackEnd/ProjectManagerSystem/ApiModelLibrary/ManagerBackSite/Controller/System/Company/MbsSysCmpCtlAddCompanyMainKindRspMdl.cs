using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司主分類-回應模型</summary>
public class MbsSysCmpCtlAddCompanyMainKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyMainKindID { get; set; }
}