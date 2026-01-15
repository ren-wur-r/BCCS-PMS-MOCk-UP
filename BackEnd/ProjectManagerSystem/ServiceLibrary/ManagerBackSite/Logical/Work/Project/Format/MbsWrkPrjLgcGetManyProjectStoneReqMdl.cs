using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-服務-取得多筆專案里程碑-請求模型</summary>
public class MbsWrkPrjLgcGetManyProjectStoneReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    public int EmployeeProjectID { get; set; }

}