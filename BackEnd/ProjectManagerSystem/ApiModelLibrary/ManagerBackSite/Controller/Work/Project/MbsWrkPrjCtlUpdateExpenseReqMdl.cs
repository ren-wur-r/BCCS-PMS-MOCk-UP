using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-控制器-更新專案支出-請求模型</summary>
public class MbsWrkPrjCtlUpdateExpenseReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案支出ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectExpenseID { get; set; }

    /// <summary>員工專案支出-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectExpenseName { get; set; }

    /// <summary>員工專案支出-預估金額</summary>
    [Required]
    [JsonPropertyName("c")]
    public decimal EmployeeProjectExpensePredictAmount { get; set; }

    /// <summary>員工專案支出-實際金額</summary>
    [JsonPropertyName("d")]
    public decimal? EmployeeProjectExpenseActualAmount { get; set; }

    /// <summary>員工專案支出-發票號碼</summary>
    [JsonPropertyName("e")]
    public string EmployeeProjectExpenseBillNumber { get; set; }

    /// <summary>員工專案支出-發票日期</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset? EmployeeProjectExpenseBillTime { get; set; }

    /// <summary>員工專案支出-備註</summary>
    [JsonPropertyName("g")]
    public string EmployeeProjectExpenseRemark { get; set; }
}