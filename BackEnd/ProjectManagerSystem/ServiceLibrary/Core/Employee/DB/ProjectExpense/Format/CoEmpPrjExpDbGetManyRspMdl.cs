using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectExpense.Format;

/// <summary>核心-員工-專案支出-資料庫-取得多筆-回應模型</summary>
public class CoEmpPrjExpDbGetManyRspMdl
{
    /// <summary>員工專案支出列表</summary>
    public List<CoEmpPrjExpDbGetManyRspItemMdl> EmployeeProjectExpenseList { get; set; }
}