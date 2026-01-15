using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆電銷產品-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelineProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>電銷產品列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPhnCtlGetManyEmployeePipelineProductRspItemMdl> EmployeePipelineProductList { get; set; }
}
