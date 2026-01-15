using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-取得多筆-請求模型</summary>
public class CoEmpPwDbGetManyRspMdl
{
    /// <summary>員工專案工作計劃書列表</summary>
    public List<CoEmpPwDbGetManyRspItemMdl> EmployeeProjectWorkList { get; set; }
}