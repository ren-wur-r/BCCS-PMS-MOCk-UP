using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.EmployeeProjectMember;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-轉換商機至專案-項目-請求模型</summary>
public class MbsCrmPplCtlTransferPipelineToProjectReqItemMdl
{
    /// <summary>員工ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeID { get; set; }

    /// <summary>員工專案成員角色</summary>
    [Required]
    [JsonPropertyName("b")]
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }
}
