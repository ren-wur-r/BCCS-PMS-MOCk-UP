using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-更新名單窗口-請求模型</summary>
public class MbsCrmPplCtlUpdateEmployeePipelineContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機窗口ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineContacterID { get; set; }

    /// <summary>窗口ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerContacterID { get; set; }

    /// <summary>商機窗口是否為主要窗口</summary>
    [Required]
    [JsonPropertyName("c")]
    public bool EmployeePipelineContacterIsPrimary { get; set; }
}
