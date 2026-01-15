using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-刪除名單窗口-請求模型</summary>
public class MbsCrmPhnCtlRemoveEmployeePipelineContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機窗口ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineContacterID { get; set; }
}
