using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;
public class CoEmpPrjDbGetDashboardFromMbsWrkPrjRspItemMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>管理公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案開始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案結束時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

}
