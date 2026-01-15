using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-刪除商機履約期限-請求模型</summary>
public class MbsCrmPplCtlRemoveEmployeePipelineDueReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機履約通知ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineDueID { get; set; }
}
