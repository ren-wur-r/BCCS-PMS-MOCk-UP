using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案成員-資料庫-取得多筆從[管理後台-工作-專案]-請求模型</summary>
public class CoEmpPrjDbGetManyFromMbsWrkPrjReqMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>頁數</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}