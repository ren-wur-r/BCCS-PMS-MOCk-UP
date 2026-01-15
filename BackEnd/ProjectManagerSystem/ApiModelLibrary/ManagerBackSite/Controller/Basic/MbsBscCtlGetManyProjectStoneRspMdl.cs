using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-專案里程碑-取得多筆-回應模型</summary>
public class MbsBscCtlGetManyProjectStoneRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案里程碑-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyProjectStoneRspItemMdl> EmployeeProjectStoneList { get; set; }
}