using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

public class CoEmpPrjDbGetRspMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工商機ID</summary>
    public int? EmployeePipelineID { get; set; }

    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案代碼</summary>
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>起始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>迄止時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }
}