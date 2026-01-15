using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-更新-請求模型</summary>
public class CoEmpPrjDbUpdateReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>管理者公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案代碼</summary>
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案起始時間</summary>
    public DateTimeOffset? EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    public DateTimeOffset? EmployeeProjectEndTime { get; set; }
}
