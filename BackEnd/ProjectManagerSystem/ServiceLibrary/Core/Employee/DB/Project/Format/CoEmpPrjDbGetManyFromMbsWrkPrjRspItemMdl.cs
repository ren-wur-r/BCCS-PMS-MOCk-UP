using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-取得多筆從[管理後台-工作-專案]-回應項目模型</summary>
public class CoEmpPrjDbGetManyFromMbsWrkPrjRspItemMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>管理公司名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>員工專案開始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案結束時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }
}