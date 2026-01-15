using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆管理者公司-回應模型</summary>
public class MbsBscCtlGetManyCompanyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyCompanyRspItemMdl> ManagerCompanyList { get; set; }
}
