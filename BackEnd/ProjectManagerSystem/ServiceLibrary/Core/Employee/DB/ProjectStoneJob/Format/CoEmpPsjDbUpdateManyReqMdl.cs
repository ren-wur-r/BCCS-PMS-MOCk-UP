using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-更新多筆-請求模型</summary>
public class CoEmpPsjDbUpdateManyReqMdl
{
    /// <summary>員工-專案里程碑工項-ID-列表</summary>
    public List<int> EmployeeProjectStoneJobIdList { get; set; }

    /// <summary>原子-員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }

}