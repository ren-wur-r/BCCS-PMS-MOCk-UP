using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆商機業務-請求模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineSalerReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務-狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomEmployeePipelineSalerStatusEnum? EmployeePipelineSalerStatus { get; set; }
}
