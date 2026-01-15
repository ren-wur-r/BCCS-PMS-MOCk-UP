using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-取得專案支出-請求模型</summary>
public class MbsWrkPrjLgcGetExpenseReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案支出ID</summary>
    public int EmployeeProjectExpenseID { get; set; }
}