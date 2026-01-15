using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineBill;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-發票紀錄項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl
{
    /// <summary>商機發票紀錄ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineBillID { get; set; }

    /// <summary>發票期數</summary>
    [JsonPropertyName("b")]
    public short EmployeePipelineBillPeriodNumber { get; set; }

    /// <summary>發票日期</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeePipelineBillBillTime { get; set; }

    /// <summary>發票號碼</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineBillBillNumber { get; set; }

    /// <summary>未稅發票金額</summary>
    [JsonPropertyName("e")]
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>備註</summary>
    [JsonPropertyName("f")]
    public string EmployeePipelineBillRemark { get; set; }

    /// <summary>發票狀態</summary>
    [JsonPropertyName("g")]
    public DbAtomEmployeePipelineBillStatusEnum EmployeePipelineBillStatus { get; set; }
}
