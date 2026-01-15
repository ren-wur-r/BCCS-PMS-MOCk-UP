using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆名單窗口-回應模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機窗口列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPplCtlGetManyEmployeePipelineContacterRspItemMdl> EmployeePipelineContacterList { get; set; }
}
