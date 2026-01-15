using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得儀錶板從[管理後台-工作-專案]-回應模型</summary>
public class CoEmpPrjDbGetDashboardFromMbsWrkPrjRspMdl
{
    /// <summary>員工專案清單</summary>
    public List<CoEmpPrjDbGetDashboardFromMbsWrkPrjRspItemMdl> EmployeeProjectList { get; set; }
}
