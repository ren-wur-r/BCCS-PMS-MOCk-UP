using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增商機履約期限-回應模型</summary>
public class MbsCrmPplCtlAddEmployeePipelineDueRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機履約通知ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineDueID { get; set; }
}
