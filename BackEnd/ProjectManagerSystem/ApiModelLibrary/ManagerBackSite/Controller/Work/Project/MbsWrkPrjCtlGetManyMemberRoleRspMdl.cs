using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆成員角色-回應模型</summary>
public class MbsWrkPrjCtlGetManyMemberRoleRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案成員清單</summary>
    [JsonPropertyName("a")]
    public List<MbsWrkPrjCtlGetManyMemberRoleRspItemMdl> EmployeeProjectMemberList { get; set; }

}
