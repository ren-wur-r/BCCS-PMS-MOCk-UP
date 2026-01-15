using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動產品-控制器-取得多筆-回應模型</summary>
public class MbsCrmActCtlGetManyActivityProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動產品列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlGetManyActivityProductRspItemMdl> ManagerActivityProductList { get; set; }
}
