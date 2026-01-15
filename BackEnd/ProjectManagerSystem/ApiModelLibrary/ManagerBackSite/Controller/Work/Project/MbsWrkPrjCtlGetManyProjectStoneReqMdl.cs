using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑-請求模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }
}