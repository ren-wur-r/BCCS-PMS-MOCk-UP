using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得儀錶板從[管理後台-工作-專案]-請求模型</summary>
public class CoEmpPrjDbGetDashboardFromMbsWrkPrjReqMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工專案狀態列表</summary>
    public List<DbAtomEmployeeProjectStatusEnum> AtomEmployeeProjectStatusList { get; set; }

}
