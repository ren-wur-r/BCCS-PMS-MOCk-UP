using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>
/// 管理者後台-CRM-商機管理-控制器-新增商機-商機履約期限-請求項目模型
/// </summary>
public class MbsCrmPplCtlAddEmployeePipelineReqDueItemMdl
{
    /// <summary>
    /// 商機履約期限-履約日期
    /// </summary>
    [Required]
    [JsonPropertyName("a")]
    public DateTimeOffset EmployeePipelineDueTime { get; set; }

    /// <summary>
    /// 商機履約期限-備註
    /// </summary>
    [JsonPropertyName("b")]
    [StringLength(100, ErrorMessage = "Invalid parameters")]
    public string EmployeePipelineDueRemark { get; set; }
}
