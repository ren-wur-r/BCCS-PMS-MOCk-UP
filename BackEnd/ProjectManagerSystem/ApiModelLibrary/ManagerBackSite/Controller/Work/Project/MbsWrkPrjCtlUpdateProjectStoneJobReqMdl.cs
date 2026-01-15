using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-更新專案里程碑工項-請求模型</summary>
public class MbsWrkPrjCtlUpdateProjectStoneJobReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案里程碑工項ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工專案里程碑工項備註</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工專案里程碑工項執行者ID列表(員工ID列表,更新方式,刪掉再新增)</summary>
    [JsonPropertyName("c")]
    public List<int> EmployeeProjectStoneJobExecutorIdList { get; set; }

    /// <summary>員工專案里程碑工項清單列表(工項清單-ID, >0:更新, -1:新增)</summary>
    [JsonPropertyName("d")]
    public List<MbsWrkPrjCtlUpdateProjectStoneJobReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }
}
