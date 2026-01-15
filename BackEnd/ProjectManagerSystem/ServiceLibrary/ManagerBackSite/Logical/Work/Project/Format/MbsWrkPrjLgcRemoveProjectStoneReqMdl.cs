using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-邏輯-工作-專案-格式-移除專案里程碑-請求模型</summary>
public class MbsWrkPrjLgcRemoveProjectStoneReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }
}