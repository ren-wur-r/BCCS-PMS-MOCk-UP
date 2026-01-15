using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑-回應模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-專案里程碑-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl> EmployeeProjectStoneList { get; set; }
}