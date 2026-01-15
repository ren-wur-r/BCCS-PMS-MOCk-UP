using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案支出-回應項目模型</summary>
public class MbsWrkPrjCtlGetManyExpenseRspItemMdl
{
    /// <summary>員工專案支出ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectExpenseID { get; set; }

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