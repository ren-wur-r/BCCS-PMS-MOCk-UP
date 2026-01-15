using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-更新專案里程碑工項-請求模型</summary>
public class MbsWrkJobCtlUpdateProjectStoneJobReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    [JsonPropertyName("c")]
    public List<MbsWrkJobCtlUpdateProjectStoneJobReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

}