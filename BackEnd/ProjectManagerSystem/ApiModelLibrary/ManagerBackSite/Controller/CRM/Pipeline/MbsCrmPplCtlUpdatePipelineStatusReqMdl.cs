using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.EmployeePipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-更新商機狀態-請求模型</summary>
public class MbsCrmPplCtlUpdatePipelineStatusReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>資料庫-原子-商機-狀態-列舉</summary>
    [Required]
    [Range(7, 11, ErrorMessage = "Invalid parameters")]
    [JsonPropertyName("b")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>需求確認狀態</summary>
    [JsonPropertyName("c")]
    public DbEmployeePipelineStageCheckEnum? BusinessNeedStatus { get; set; }

    /// <summary>時程確認狀態</summary>
    [JsonPropertyName("d")]
    public DbEmployeePipelineStageCheckEnum? BusinessTimelineStatus { get; set; }

    /// <summary>預算確認狀態</summary>
    [JsonPropertyName("e")]
    public DbEmployeePipelineStageCheckEnum? BusinessBudgetStatus { get; set; }

    /// <summary>簡報確認狀態</summary>
    [JsonPropertyName("f")]
    public DbEmployeePipelineStageCheckEnum? BusinessPresentationStatus { get; set; }

    /// <summary>報價確認狀態</summary>
    [JsonPropertyName("g")]
    public DbEmployeePipelineStageCheckEnum? BusinessQuotationStatus { get; set; }

    /// <summary>議價確認狀態</summary>
    [JsonPropertyName("h")]
    public DbEmployeePipelineStageCheckEnum? BusinessNegotiationStatus { get; set; }

    /// <summary>階段備註</summary>
    [JsonPropertyName("i")]
    public string BusinessStageRemark { get; set; }
}
