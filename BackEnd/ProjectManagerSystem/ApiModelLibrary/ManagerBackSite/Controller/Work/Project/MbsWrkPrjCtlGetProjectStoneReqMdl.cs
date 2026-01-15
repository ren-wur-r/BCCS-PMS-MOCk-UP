using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑-請求模型</summary>
public class MbsWrkPrjCtlGetProjectStoneReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-專案里程碑-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneID { get; set; }
}