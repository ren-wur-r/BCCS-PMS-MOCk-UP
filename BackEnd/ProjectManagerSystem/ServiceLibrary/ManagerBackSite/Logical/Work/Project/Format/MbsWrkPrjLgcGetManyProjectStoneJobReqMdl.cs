using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項-請求模型</summary>
public class MbsWrkPrjLgcGetManyProjectStoneJobReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }
}
