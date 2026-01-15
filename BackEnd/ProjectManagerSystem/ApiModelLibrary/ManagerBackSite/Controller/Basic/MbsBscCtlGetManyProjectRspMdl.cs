using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆員工專案-回應模型</summary>
public class MbsBscCtlGetManyProjectRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyProjectRspItemMdl> EmployeeProjectList { get; set; }
}