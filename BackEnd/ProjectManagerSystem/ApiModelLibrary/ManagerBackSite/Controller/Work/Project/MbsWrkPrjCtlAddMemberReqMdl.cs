using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.EmployeeProjectMember;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-控制器-新增專案成員-請求模型</summary>
public class MbsWrkPrjCtlAddMemberReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int EmployeeID { get; set; }

    /// <summary>員工專案成員角色-列舉</summary>
    [Required]
    [JsonPropertyName("c")]
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }
}
