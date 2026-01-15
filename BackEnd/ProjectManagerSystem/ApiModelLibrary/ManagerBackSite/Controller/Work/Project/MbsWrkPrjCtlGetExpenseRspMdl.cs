using System;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案支出-回應模型</summary>
public class MbsWrkPrjCtlGetExpenseRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案支出名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectExpenseName { get; set; }

    /// <summary>員工專案支出預估金額</summary>
    [JsonPropertyName("c")]
    public decimal EmployeeProjectExpensePredictAmount { get; set; }

    /// <summary>員工專案支出實際金額</summary>
    [JsonPropertyName("d")]
    public decimal? EmployeeProjectExpenseActualAmount { get; set; }

    /// <summary>員工專案支出發票號碼</summary>
    [JsonPropertyName("e")]
    public string EmployeeProjectExpenseBillNumber { get; set; }

    /// <summary>員工專案支出發票日期</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset? EmployeeProjectExpenseBillTime { get; set; }

    /// <summary>員工專案支出備註</summary>
    [JsonPropertyName("g")]
    public string EmployeeProjectExpenseRemark { get; set; }
}