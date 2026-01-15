using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增活動名單-回應模型</summary>
public class MbsCrmActCtlAddActivityEmployeePipelineRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工商機ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }
}
