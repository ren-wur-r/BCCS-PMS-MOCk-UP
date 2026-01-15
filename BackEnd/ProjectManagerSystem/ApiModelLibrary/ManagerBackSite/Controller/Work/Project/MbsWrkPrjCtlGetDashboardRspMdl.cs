using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得儀表板-回應模型</summary>
public class MbsWrkPrjCtlGetDashboardRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>延遲員工專案列表</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkPrjCtlGetDashboardRspItemMdl> DelayedEmployeeProjectList { get; set; }

    /// <summary>注意員工專案列表</summary>
    [JsonPropertyName("b")]
    public List<MbsWrkPrjCtlGetDashboardRspItemMdl> AtRiskEmployeeProjectList { get; set; }

    /// <summary>未指派員工專案列表</summary>
    [JsonPropertyName("c")]
    public List<MbsWrkPrjCtlGetDashboardRspItemMdl> NotAssignedEmployeeProjectList { get; set; }
}
