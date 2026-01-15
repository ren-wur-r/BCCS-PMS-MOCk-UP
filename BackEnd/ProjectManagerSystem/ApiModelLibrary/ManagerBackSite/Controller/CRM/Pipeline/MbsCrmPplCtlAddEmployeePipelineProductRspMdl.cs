using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增商機產品-回應模型</summary>
public class MbsCrmPplCtlAddEmployeePipelineProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機產品ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }
}