using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆名單-請求模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機狀態</summary>
    [JsonPropertyName("a")]
    public DbAtomPipelineStatusEnum? AtomPipelineStatus { get; set; }

    /// <summary>管理者公司名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyName { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    [JsonPropertyName("e")]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [JsonPropertyName("f")]
    public int PageSize { get; set; }

    /// <summary>員工商機-業務員工ID (查詢條件)</summary>
    [JsonPropertyName("g")]
    public int? EmployeePipelineSalerEmployeeID { get; set; }
}
