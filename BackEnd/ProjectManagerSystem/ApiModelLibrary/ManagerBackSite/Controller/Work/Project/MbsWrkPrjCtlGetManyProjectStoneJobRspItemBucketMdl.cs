using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應清單項目模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneJobRspItemBucketMdl
{
    /// <summary>員工專案里程碑工項清單ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>員工專案里程碑工項清單名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>員工專案里程碑工項清單是否完成</summary>
    [JsonPropertyName("c")]
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
