using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>
/// 管理者後台-CRM-商機管理-控制器-新增商機-商機業務開發紀錄-請求項目模型
/// </summary>
public class MbsCrmPplCtlAddEmployeePipelineReqSalerTrackingItemMdl
{
    /// <summary>
    /// 商機業務開發紀錄-開發時間
    /// </summary>
    [Required]
    [JsonPropertyName("a")]
    public DateTimeOffset EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>
    /// 管理者窗口ID
    /// </summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerContacterID { get; set; }

    /// <summary>
    /// 商機業務開發紀錄-備註
    /// </summary>    
    [JsonPropertyName("c")]
    [StringLength(2000, ErrorMessage = "Invalid parameters")]
    public string EmployeePipelineSalerTrackingRemark { get; set; }
}
