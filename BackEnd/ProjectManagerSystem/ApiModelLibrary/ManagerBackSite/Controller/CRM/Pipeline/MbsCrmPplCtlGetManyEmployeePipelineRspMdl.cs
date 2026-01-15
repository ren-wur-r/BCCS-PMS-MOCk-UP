using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆名單-回應模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl> EmployeePipelineList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
