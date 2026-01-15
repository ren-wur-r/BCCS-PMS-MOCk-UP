using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeePipelineBill;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-更新商機發票紀錄-請求模型</summary>
public class MbsCrmPplCtlUpdateEmployeePipelineBillReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機發票紀錄ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineBillID { get; set; }

    /// <summary>發票號碼</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeePipelineBillNumber { get; set; }

    /// <summary>發票狀態</summary>
    [Required]
    [JsonPropertyName("c")]
    public DbAtomEmployeePipelineBillStatusEnum? EmployeePipelineBillStatus { get; set; }

    /// <summary>備註</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineBillRemark { get; set; }
}