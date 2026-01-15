using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-刪除商機產品-請求模型</summary>
public class MbsCrmPplCtlRemoveEmployeePipelineProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機產品ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }
}