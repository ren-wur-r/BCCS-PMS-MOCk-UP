using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆關係公司-回應模型</summary>
public class MbsSysCmpCtlGetManyCompanyAffiliateRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者關係公司-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysCmpCtlGetManyCompanyAffiliateRspItemMdl> ManagerCompanyAffiliateList { get; set; }
}
