using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-新增專案里程碑工項工作-請求項目檔案模型</summary>
public class MbsWrkJobCtlAddProjectStoneJobWorkReqItemFileMdl
{
    /// <summary>員工-專案里程碑工項工作檔案-網址</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectStoneJobWorkFileUrl { get; set; }
}