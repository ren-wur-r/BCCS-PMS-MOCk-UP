using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>
/// 管理者後台-CRM-商機管理-控制器-新增商機-聯絡人-請求項目模型
/// </summary>
public class MbsCrmPplCtlAddEmployeePipelineReqContacterItemMdl
{
    /// <summary>
    /// 管理者聯絡人ID
    /// </summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>
    /// 員工商機聯絡人是否為主要
    /// </summary>
    [Required]
    [JsonPropertyName("b")]
    public bool EmployeePipelineContacterIsPrimary { get; set; }
}
