using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-刪除多筆活動名單-請求模型</summary>
public class MbsCrmActCtlRemoveManyActivityEmployeePipelineReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID列表</summary>
    [Required]
    [JsonPropertyName("a")]
    public List<int> EmployeePipelineIDList { get; set; }
}
