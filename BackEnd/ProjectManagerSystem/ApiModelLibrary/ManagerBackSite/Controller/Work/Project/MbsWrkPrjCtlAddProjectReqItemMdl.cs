using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.EmployeeProjectMember;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-新增專案-請求項目模型</summary>
public class MbsWrkPrjCtlAddProjectReqItemMdl
{
    /// <summary>員工專案成員角色</summary>
    [Required]
    [JsonPropertyName("a")]
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

    /// <summary>員工ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int EmployeeID { get; set; }
}