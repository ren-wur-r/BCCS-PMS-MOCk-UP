using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得多筆-回應模型</summary>
public class CoEmpPrjDbGetManyIdRspMdl
{
    /// <summary>員工專案-列表</summary>
    public List<CoEmpPrjDbGetManyIdRspItemMdl> EmployeeProjectList { get; set; }
}