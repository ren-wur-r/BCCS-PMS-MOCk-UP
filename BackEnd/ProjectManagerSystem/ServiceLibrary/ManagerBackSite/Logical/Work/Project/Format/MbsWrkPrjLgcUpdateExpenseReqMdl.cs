using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-更新專案支出-請求模型</summary>
public class MbsWrkPrjLgcUpdateExpenseReqMdl : MbsLgcBaseReqMdl
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