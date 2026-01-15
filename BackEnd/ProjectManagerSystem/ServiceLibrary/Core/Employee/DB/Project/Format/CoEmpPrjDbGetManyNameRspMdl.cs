using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得多筆[名稱]-回應模型</summary>
public class CoEmpPrjDbGetManyNameRspMdl
{
    /// <summary>員工-專案-列表</summary>
    public List<CoEmpPrjDbGetManyNameRspItemMdl> EmployeeProjectList { get; set; }
}