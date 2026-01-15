using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-處理商機業務-請求模型</summary>
public class MbsCrmPplCtlHandleEmployeePipelineSalerReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務-狀態 (限制: 2-接受, 3-拒絕, 4-轉指派業務)</summary>
    [Required]
    [Range(2, 4, ErrorMessage = "Invalid parameters")]
    [JsonPropertyName("b")]
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-備注</summary>
    [JsonPropertyName("c")]
    [StringLength(500, ErrorMessage = "Invalid parameters")]
    public string EmployeePipelineSalerRemark { get; set; }

    /// <summary>商機業務-轉指派業務員工ID</summary>
    [JsonPropertyName("d")]
    public int? EmployeePipelineSalerEmployeeID { get; set; }
}
