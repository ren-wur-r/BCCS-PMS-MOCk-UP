using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項-請求模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }
}