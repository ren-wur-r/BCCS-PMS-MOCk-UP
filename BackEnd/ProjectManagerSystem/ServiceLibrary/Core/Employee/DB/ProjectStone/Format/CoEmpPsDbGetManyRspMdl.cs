using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-資料庫-取得多筆-回應模型</summary>
public class CoEmpPsDbGetManyRspMdl
{
    /// <summary>員工專案里程碑-列表</summary>
    public List<CoEmpPsDbGetManyRspItemMdl> EmployeeProjectStoneList { get; set; }
}