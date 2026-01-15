using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆指派業務-項目-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl
{
    /// <summary>商機業務ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>商機業務-狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-建立時間(指派時間)</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeePipelineSalerCreateTime { get; set; }

    /// <summary>商機業務-指派人員名稱</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineSalerCreateEmployeeName { get; set; }

    /// <summary>商機業務-業務回覆時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset? EmployeePipelineSalerReplyTime { get; set; }

    /// <summary>商機業務-業務員工名稱(回覆業務人員)</summary>
    [JsonPropertyName("f")]
    public string EmployeePipelineSalerEmployeeName { get; set; }  

    /// <summary>商機業務-備注</summary>
    [JsonPropertyName("g")]
    public string EmployeePipelineSalerRemark { get; set; }
}
