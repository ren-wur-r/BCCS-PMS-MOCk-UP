using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增商機業務開發紀錄-回應模型</summary>
public class MbsCrmPplCtlAddEmployeePipelineSalerTrackingRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機業務開發紀錄ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineSalerTrackingID { get; set; }
}
