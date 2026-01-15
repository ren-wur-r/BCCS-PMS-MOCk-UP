using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-更新專案里程碑工項-工項清單項目-請求模型</summary>
public class MbsWrkPrjCtlUpdateProjectStoneJobReqItemBucketMdl
{
    /// <summary>專案里程碑工項清單ID (>0:更新, -1:新增)</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>專案里程碑工項清單名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>專案里程碑工項清單是否完成</summary>
    [Required]
    [JsonPropertyName("c")]
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
