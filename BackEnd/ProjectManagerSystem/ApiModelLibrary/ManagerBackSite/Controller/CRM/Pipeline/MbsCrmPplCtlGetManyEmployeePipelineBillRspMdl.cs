using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆商機發票紀錄-回應模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineBillRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機發票紀錄列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl> EmployeePipelineBillList { get; set; }
}