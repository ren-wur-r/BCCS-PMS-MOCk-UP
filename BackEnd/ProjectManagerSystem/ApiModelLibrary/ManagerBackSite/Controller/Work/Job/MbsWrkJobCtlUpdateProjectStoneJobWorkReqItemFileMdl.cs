using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-更新專案里程碑工項工作-請求項目檔案模型</summary>
public class MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemFileMdl
{
    /// <summary>員工-專案里程碑工項工作檔案-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobWorkFileID { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-網址</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobWorkFileUrl { get; set; }
}