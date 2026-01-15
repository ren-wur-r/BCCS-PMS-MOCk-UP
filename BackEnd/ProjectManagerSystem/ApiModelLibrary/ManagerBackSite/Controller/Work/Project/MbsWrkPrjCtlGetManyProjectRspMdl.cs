using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案-回應模型</summary>
public class MbsWrkPrjCtlGetManyProjectRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案列表</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkPrjCtlGetManyProjectRspItemMdl> EmployeeProjectList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}
