using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-轉換商機至專案-回應模型</summary>
public class MbsCrmPplCtlTransferPipelineToProjectRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }
}
