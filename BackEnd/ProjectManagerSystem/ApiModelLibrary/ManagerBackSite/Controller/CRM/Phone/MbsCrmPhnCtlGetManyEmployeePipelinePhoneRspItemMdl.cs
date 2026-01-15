using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆電銷紀錄-項目-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl
{
    /// <summary>商機電銷紀錄ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelinePhoneID { get; set; }

    /// <summary>商機電銷紀錄-時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>商機電銷紀錄-窗口名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-電銷員工名稱</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelinePhoneCreateEmployeeName { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    [JsonPropertyName("f")]
    public string EmployeePipelinePhoneRemark { get; set; }
}
