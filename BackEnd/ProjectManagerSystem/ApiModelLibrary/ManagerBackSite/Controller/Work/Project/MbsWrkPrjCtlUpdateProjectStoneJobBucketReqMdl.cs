using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-更新專案里程碑工項清單-請求模型</summary>
public class MbsWrkPrjCtlUpdateProjectStoneJobBucketReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>專案里程碑工項清單ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>是否完成</summary>
    [Required]
    [JsonPropertyName("b")]
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
