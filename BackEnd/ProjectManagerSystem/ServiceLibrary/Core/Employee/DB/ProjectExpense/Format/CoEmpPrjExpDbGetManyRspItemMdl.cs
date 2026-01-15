using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectExpense.Format;

/// <summary>核心-員工-專案支出-資料庫-取得多筆-回應-項目</summary>
public class CoEmpPrjExpDbGetManyRspItemMdl
{
    /// <summary>員工專案支出ID</summary>
    public int EmployeeProjectExpenseID { get; set; }

    /// <summary>員工專案支出-名稱</summary>
    public string EmployeeProjectExpenseName { get; set; }

    /// <summary>員工專案支出-預估金額</summary>
    public decimal EmployeeProjectExpensePredictAmount { get; set; }

    /// <summary>員工專案支出-實際金額</summary>
    public decimal? EmployeeProjectExpenseActualAmount { get; set; }

    /// <summary>員工專案支出-發票號碼</summary>
    public string EmployeeProjectExpenseBillNumber { get; set; }

    /// <summary>員工專案支出-發票日期</summary>
    public DateTimeOffset? EmployeeProjectExpenseBillTime { get; set; }

    /// <summary>員工專案支出-備註</summary>
    public string EmployeeProjectExpenseRemark { get; set; }
}