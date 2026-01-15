using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司營業地點-回應模型</summary>
public class MbsSysCmpCtlGetManyCompanyLocationRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司營業地點-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysCmpCtlGetManyCompanyLocationRspItemMdl> ManagerCompanyLocationList { get; set; }
}
