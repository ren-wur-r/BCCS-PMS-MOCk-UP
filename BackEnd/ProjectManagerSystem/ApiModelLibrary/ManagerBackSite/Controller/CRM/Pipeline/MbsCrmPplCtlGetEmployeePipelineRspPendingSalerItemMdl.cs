using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-指派業務紀錄項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspPendingSalerItemMdl
{
    /// <summary>商機業務ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>商機業務-業務員工名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeePipelineSalerEmployeeName { get; set; }

    /// <summary>商機業務-建立時間(指派時間)</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeePipelineSalerCreateTime { get; set; }

    /// <summary>商機業務-狀態</summary>
    [JsonPropertyName("d")]
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-建立員工名稱(指派人員)</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelineSalerCreateEmployeeName { get; set; }

    /// <summary>商機業務-是否有拒絕權限</summary>
    [JsonPropertyName("f")]
    public bool HasRejectPermissions { get; set; }

    /// <summary>商機業務-是否有接受權限</summary>
    [JsonPropertyName("g")]
    public bool HasAcceptPermissions { get; set; }

    /// <summary>商機業務-是否有轉指派權限</summary>
    [JsonPropertyName("h")]
    public bool HasReassignPermissions { get; set; }
}
