using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-取得專案支出-回應模型</summary>
public class MbsWrkPrjLgcGetExpenseRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案支出名稱</summary>
    public string EmployeeProjectExpenseName { get; set; }

    /// <summary>員工專案支出預估金額</summary>
    public decimal EmployeeProjectExpensePredictAmount { get; set; }

    /// <summary>員工專案支出實際金額</summary>
    public decimal? EmployeeProjectExpenseActualAmount { get; set; }

    /// <summary>員工專案支出發票號碼</summary>
    public string EmployeeProjectExpenseBillNumber { get; set; }

    /// <summary>員工專案支出發票日期</summary>
    public DateTimeOffset? EmployeeProjectExpenseBillTime { get; set; }

    /// <summary>員工專案支出備註</summary>
    public string EmployeeProjectExpenseRemark { get; set; }
}