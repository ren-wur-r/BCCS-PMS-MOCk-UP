using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-取得多筆-回應模型</summary>
public class MbsSysEmpCtlGetManyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysEmpCtlGetManyRspItemMdl> EmployeeList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}