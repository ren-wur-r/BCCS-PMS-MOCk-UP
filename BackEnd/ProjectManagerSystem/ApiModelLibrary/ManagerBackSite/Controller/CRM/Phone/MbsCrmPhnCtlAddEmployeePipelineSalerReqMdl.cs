using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-指派業務-請求模型</summary>
public class MbsCrmPhnCtlAddEmployeePipelineSalerReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機業務-員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務-業務員工ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int EmployeePipelineSalerEmployeeID { get; set; }
}
