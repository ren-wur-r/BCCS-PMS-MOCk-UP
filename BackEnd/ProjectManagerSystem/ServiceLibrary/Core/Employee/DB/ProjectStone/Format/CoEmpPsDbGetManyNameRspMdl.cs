using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-取得多筆[名稱]-回應模型</summary>
public class CoEmpPsDbGetManyNameRspMdl
{
    /// <summary>員工-專案里程碑-列表</summary>
    public List<CoEmpPsDbGetManyNameRspItemMdl> EmployeeProjectStoneList { get; set; } = new();
}