using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-邏輯-取得儀表板-回應模型</summary>
public class MbsWrkPrjLgcGetDashboardRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>延遲員工專案列表</summary>
    public List<MbsWrkPrjLgcGetDashboardRspItemMdl> DelayedEmployeeProjectList { get; set; }

    /// <summary>注意員工專案列表</summary>
    public List<MbsWrkPrjLgcGetDashboardRspItemMdl> AtRiskEmployeeProjectList { get; set; }

    /// <summary>未指派員工專案列表</summary>
    public List<MbsWrkPrjLgcGetDashboardRspItemMdl> NotAssignedEmployeeProjectList { get; set; }

}
