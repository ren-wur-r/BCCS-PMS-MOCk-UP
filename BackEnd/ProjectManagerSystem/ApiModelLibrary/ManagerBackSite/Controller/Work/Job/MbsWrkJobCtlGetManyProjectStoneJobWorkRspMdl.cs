using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作-回應模型</summary>
public class MbsWrkJobCtlGetManyProjectStoneJobWorkRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-專案里程碑工項工作-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkJobCtlGetManyProjectStoneJobWorkRspItemMdl> EmployeeProjectStoneJobWorkList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}