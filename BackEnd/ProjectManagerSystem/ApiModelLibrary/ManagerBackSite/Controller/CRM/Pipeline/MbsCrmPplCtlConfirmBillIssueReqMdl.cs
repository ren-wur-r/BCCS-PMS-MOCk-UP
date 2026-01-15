using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-確認開立發票-請求模型</summary>
public class MbsCrmPplCtlConfirmBillIssueReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機發票ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineBillID { get; set; }

    /// <summary>員工商機發票號碼</summary>
    [JsonPropertyName("b")]
    public string EmployeePipelineBillNumber { get; set; }

    /// <summary>員工商機發票備註</summary>
    [JsonPropertyName("c")]
    public string EmployeePipelineBillRemark { get; set; }
}
