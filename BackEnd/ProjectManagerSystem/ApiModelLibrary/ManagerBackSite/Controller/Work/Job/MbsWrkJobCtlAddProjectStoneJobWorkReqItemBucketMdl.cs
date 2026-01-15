using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-新增專案里程碑工項工作-請求項目清單模型</summary>
public class MbsWrkJobCtlAddProjectStoneJobWorkReqItemBucketMdl
{
    /// <summary>員工-專案里程碑工項清單-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>員工-專案里程碑工項清單-是否完成</summary>
    [JsonPropertyName("b")]
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}