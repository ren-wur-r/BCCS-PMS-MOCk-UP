using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-控制器-移除專案成員-請求模型</summary>
public class MbsWrkPrjCtlRemoveMemberReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案成員角色-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectMemberID { get; set; }

}
