using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增多筆商機發票紀錄-請求模型</summary>
public class MbsCrmPplCtlAddManyEmployeePipelineBillReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機發票紀錄列表</summary>
    [Required]
    [JsonPropertyName("b")]
    public List<MbsCrmPplCtlAddManyEmployeePipelineBillReqItemMdl> EmployeePipelineBillList { get; set; }
}