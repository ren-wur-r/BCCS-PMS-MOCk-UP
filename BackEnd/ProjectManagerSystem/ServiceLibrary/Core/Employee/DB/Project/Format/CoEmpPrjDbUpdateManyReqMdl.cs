using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-更新多筆-請求模型</summary>
public class CoEmpPrjDbUpdateManyReqMdl
{
    /// <summary>員工-專案-ID-列表</summary>
    public List<int> EmployeeProjectIdList { get; set; }

    /// <summary>員工-專案-狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }
}