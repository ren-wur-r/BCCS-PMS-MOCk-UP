using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案石-資料庫-更新多筆-請求模型</summary>
public class CoEmpPsDbUpdateManyReqMdl
{
    /// <summary>員工-專案里程碑-ID-列表</summary>
    public List<int> EmployeeProjectStoneIdList { get; set; }

    /// <summary>原子員工專案-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }
}