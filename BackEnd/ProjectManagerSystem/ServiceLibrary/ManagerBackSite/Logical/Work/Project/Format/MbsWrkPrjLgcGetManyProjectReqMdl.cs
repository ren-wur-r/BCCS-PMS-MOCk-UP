using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆人員-請求模型</summary>
public class MbsWrkPrjLgcGetManyProjectReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum? AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>頁數</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}