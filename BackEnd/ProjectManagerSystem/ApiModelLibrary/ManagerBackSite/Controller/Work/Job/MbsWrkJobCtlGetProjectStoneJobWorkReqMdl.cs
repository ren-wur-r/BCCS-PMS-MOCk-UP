using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobWorkReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobWorkID { get; set; }
}