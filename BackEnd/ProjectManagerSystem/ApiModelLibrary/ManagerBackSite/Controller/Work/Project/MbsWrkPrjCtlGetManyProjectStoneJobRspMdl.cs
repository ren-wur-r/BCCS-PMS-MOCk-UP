using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneJobRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案里程碑列表</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkPrjCtlGetManyProjectStoneJobRspItemStoneMdl> EmployeeProjectStoneList { get; set; }
}
