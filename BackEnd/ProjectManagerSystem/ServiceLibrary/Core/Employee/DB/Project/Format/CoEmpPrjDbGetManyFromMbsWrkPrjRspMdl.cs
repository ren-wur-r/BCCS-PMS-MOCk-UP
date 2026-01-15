using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-取得多筆從[管理後台-工作-專案]-回應模型</summary>
public class CoEmpPrjDbGetManyFromMbsWrkPrjRspMdl
{
    /// <summary>員工專案列表</summary>
    public List<CoEmpPrjDbGetManyFromMbsWrkPrjRspItemMdl> EmployeeProjectList { get; set; }
}