using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆活動名單-回應模型</summary>
public class MbsCrmActCtlGetManyActivityEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工商機列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl> EmployeePipelineList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
