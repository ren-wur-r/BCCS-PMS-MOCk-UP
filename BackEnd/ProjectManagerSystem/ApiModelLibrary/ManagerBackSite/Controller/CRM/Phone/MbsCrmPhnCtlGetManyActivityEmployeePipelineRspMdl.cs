using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆名單-回應模型</summary>
public class MbsCrmPhnCtlGetManyActivityEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>名單列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPhnCtlGetManyEmployeePipelineRspItemMdl> EmployeePipelineList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
