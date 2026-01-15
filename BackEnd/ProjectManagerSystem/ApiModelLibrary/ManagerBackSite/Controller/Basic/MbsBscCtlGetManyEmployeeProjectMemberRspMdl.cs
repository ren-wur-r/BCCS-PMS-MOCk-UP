using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員-回應模型</summary>
public class MbsBscCtlGetManyEmployeeProjectMemberRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-專案成員-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl> EmployeeProjectMemberList { get; set; }
}
