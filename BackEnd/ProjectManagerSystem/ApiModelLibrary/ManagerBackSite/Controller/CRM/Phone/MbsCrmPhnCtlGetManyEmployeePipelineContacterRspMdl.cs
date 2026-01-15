using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆名單窗口-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelineContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>名單窗口列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl> EmployeePipelineContacterList { get; set; }
}
