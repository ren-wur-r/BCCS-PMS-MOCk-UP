using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目檔案模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobRspItemFileMdl
{
    /// <summary>員工-專案里程碑工項工作檔案-網址</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectStoneJobWorkFileUrl { get; set; }
}