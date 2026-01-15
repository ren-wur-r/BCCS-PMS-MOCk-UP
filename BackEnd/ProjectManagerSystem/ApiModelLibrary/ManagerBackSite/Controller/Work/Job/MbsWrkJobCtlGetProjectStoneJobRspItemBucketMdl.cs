using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項-回應項目清單模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobRspItemBucketMdl
{
    /// <summary>員工-專案里程碑工項清單-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>員工-專案里程碑工項清單-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>員工-專案里程碑工項清單-是否完成</summary>
    [JsonPropertyName("c")]
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}