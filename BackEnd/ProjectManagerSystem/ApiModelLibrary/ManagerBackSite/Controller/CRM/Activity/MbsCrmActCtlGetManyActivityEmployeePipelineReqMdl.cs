using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆活動名單-請求模型</summary>
public class MbsCrmActCtlGetManyActivityEmployeePipelineReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>活動ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }

    /// <summary>商機狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomPipelineStatusEnum? AtomPipelineStatus { get; set; }

    /// <summary>管理者公司名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyName { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("f")]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [Required]
    [JsonPropertyName("g")]
    public int PageSize { get; set; }
}
