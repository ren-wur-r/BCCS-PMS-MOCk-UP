using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目執行者模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobRspItemExecutorMdl
{
    /// <summary>員工-專案里程碑工項-執行者-名稱</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectStoneJobExecutorName { get; set; }
}