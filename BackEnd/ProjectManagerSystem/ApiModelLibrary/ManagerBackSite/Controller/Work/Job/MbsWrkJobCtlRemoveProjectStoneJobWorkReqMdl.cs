using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-移除專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobCtlRemoveProjectStoneJobWorkReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案里程碑工項工作ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobWorkID { get; set; }
}