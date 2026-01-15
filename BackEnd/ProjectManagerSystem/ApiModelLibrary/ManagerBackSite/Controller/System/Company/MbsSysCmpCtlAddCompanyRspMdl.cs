using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司-回應模型</summary>
public class MbsSysCmpCtlAddCompanyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }
}