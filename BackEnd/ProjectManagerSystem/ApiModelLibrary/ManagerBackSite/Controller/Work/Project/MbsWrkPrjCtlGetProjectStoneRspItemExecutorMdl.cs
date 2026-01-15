using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑-回應項目執行者模型</summary>
public class MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl
{
    /// <summary>員工-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeID { get; set; }

    /// <summary>員工-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeName { get; set; }
}