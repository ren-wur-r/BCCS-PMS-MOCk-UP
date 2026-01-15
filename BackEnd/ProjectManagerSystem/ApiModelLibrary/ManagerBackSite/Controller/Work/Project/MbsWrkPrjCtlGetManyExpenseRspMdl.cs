using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案支出-回應模型</summary>
public class MbsWrkPrjCtlGetManyExpenseRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案支出列表</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkPrjCtlGetManyExpenseRspItemMdl> EmployeeProjectExpenseList { get; set; }

    /// <summary>Eip專案支出列表</summary>
    [JsonPropertyName("b")]
    public List<MbsWrkPrjCtlGetEipExpenseRspItemMdl> EipProjectExpenseList { get; set; }

    /// <summary>Eip專案旅費列表</summary>
    [JsonPropertyName("c")]
    public List<MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl> EipProjectTravelExpenseList { get; set; }
}