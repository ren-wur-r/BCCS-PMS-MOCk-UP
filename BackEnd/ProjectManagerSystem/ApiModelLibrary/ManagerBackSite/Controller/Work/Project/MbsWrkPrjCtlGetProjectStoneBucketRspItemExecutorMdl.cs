using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑工項-回應執行者項目模型</summary>
public class MbsWrkPrjCtlGetProjectStoneBucketRspItemExecutorMdl
{
    /// <summary>員工專案里程碑工項執行者員工ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobExecutorEmployeeID { get; set; }

    /// <summary>員工專案里程碑工項執行者員工名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobExecutorEmployeeName { get; set; }
}
