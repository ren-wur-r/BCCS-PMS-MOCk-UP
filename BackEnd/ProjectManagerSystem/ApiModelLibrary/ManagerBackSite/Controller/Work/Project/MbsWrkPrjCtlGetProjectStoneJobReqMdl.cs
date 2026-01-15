using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑工項-請求模型</summary>
public class MbsWrkPrjCtlGetProjectStoneJobReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>專案里程碑工項ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }
}
