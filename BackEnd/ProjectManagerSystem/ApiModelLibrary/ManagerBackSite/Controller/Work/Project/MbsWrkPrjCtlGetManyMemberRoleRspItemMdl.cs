using System.Text.Json.Serialization;
using DataModelLibrary.Database.EmployeeProjectMember;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆成員角色-回應項目模型</summary>
public class MbsWrkPrjCtlGetManyMemberRoleRspItemMdl
{
    /// <summary>員工專案成員角色-列舉</summary>
    [JsonPropertyName("a")]
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }
}