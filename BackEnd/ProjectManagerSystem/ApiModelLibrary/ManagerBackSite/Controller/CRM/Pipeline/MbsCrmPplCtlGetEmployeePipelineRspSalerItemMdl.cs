using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-業務項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl
{
    /// <summary>商機業務ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>商機業務-業務員工名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeePipelineSalerEmployeeName { get; set; }

    /// <summary>商機業務-業務回覆時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset? EmployeePipelineSalerReplyTime { get; set; }

    /// <summary>商機業務-建立時間(指派時間)</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeePipelineSalerCreateTime { get; set; }

    /// <summary>商機業務-狀態</summary>
    [JsonPropertyName("e")]
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-建立員工名稱(指派人員)</summary>
    [JsonPropertyName("f")]
    public string EmployeePipelineSalerCreateEmployeeName { get; set; }

    /// <summary>商機業務-備注</summary>
    [JsonPropertyName("g")]
    public string EmployeePipelineSalerRemark { get; set; }
}
