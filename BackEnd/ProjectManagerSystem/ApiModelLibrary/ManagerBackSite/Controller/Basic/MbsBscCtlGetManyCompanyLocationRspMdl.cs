using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-展示層-取得多筆公司營業地點-回應模型</summary>
public class MbsBscCtlGetManyCompanyLocationRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司營業地點清單</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyCompanyLocationRspItemMdl> CompanyLocationList { get; set; }
}
