using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-更新專案里程碑-請求項目執行者模型</summary>
public class MbsWrkPrjCtlUpdateProjectStoneReqItemExecutorMdl
{
    /// <summary>員工-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeID { get; set; }
}