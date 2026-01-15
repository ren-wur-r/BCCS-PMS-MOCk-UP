using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoEmpPrjDbGetManyFromMbsBscRspMdl
{
    /// <summary>員工專案-列表</summary>
    public List<CoEmpPrjDbGetManyFromMbsBscRspItemMdl> EmployeeProjectList { get; set; }
}