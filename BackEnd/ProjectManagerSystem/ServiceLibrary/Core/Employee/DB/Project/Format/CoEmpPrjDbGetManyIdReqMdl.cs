using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得多筆-請求模型</summary>
public class CoEmpPrjDbGetManyIdReqMdl
{
    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }
}