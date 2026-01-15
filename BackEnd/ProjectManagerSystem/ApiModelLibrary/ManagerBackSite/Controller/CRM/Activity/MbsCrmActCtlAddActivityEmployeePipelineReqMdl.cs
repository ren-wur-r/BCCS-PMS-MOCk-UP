using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增活動名單-請求模型</summary>
public class MbsCrmActCtlAddActivityEmployeePipelineReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }

    /// <summary>客戶公司ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerCompanyID { get; set; }

    /// <summary>客戶公司營業地點ID</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>窗口ID</summary>
    [Required]
    [JsonPropertyName("d")]
    public int ManagerContacterID { get; set; }

    /// <summary>商機狀態</summary>
    [Required]
    [JsonPropertyName("e")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }
}
