using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineBill;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>
/// 管理者後台-CRM-商機管理-控制器-新增商機-商機發票紀錄-請求項目模型
/// </summary>
public class MbsCrmPplCtlAddEmployeePipelineReqBillItemMdl
{
    /// <summary>
    /// 商機發票紀錄-期數
    /// </summary>
    [Required]
    [JsonPropertyName("a")]
    public short EmployeePipelineBillPeriodNumber { get; set; }

    /// <summary>
    /// 商機發票紀錄-發票日期
    /// </summary>
    [Required]
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelineBillBillTime { get; set; }

    /// <summary>
    /// 商機發票紀錄-未稅發票金額
    /// </summary>
    [Required]
    [JsonPropertyName("c")]
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>
    /// 商機發票紀錄-備註
    /// </summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineBillRemark { get; set; }

    /// <summary>
    /// 商機發票紀錄-狀態
    /// </summary>
    [Required]
    [JsonPropertyName("e")]
    public DbAtomEmployeePipelineBillStatusEnum EmployeePipelineBillStatus { get; set; }
}
