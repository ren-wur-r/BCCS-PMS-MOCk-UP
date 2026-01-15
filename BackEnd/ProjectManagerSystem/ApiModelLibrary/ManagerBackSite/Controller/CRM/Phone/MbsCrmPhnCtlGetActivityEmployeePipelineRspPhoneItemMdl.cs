using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得活動名單-電銷項目-回應模型</summary>
public class MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl
{
    /// <summary>商機電銷紀錄ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelinePhoneID { get; set; }

    /// <summary>商機電銷紀錄-紀錄時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>窗口名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelinePhoneRemark { get; set; }

    /// <summary>商機電銷紀錄-電銷人員名稱</summary>
    [JsonPropertyName("f")]
    public string EmployeePipelinePhoneCreateEmployeeName { get; set; }
}
