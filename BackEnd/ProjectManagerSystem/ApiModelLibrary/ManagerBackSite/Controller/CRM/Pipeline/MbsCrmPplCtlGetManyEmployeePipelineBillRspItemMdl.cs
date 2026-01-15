using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆商機發票紀錄-回應項目模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl
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

    /// <summary>未稅發票金額</summary>
    [JsonPropertyName("d")]
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>備註</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelineBillRemark { get; set; }
}