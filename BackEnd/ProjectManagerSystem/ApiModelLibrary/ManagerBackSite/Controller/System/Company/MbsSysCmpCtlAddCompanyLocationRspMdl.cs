using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司營業地點-回應模型</summary>
public class MbsSysCmpCtlAddCompanyLocationRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyLocationID { get; set; }
}
