using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Department;

/// <summary>管理者後台-系統-部門-控制器-取得多筆-回應模型</summary>
public class MbsSysDptCtlGetManyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-部門列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysDptCtlGetManyRspItemMdl> ManagerDepartmentList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}