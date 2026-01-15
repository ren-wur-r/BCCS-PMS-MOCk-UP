using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-匯入Teams-項目-回應模型</summary>
public class MbsCrmActCtlImportTeamsRspItemMdl
{
    /// <summary>Teams名稱</summary>
    [JsonPropertyName("a")]
    public string TeamsName { get; set; }

    /// <summary>Teams首次加入時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset? TeamsFirstJoinTime { get; set; }

    /// <summary>Teams上次離開時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset? TeamsLastLeaveTime { get; set; }

    /// <summary>Teams會議持續時間</summary>
    [JsonPropertyName("d")]
    public string TeamsMeetingDuration { get; set; }

    /// <summary>Teams電子郵件</summary>
    [JsonPropertyName("e")]
    public string TeamsEmail { get; set; }

    /// <summary>參與者識別碼 (UPN)</summary>
    [JsonPropertyName("f")]
    public string TeamsParticipantId { get; set; }

    /// <summary>Teams角色</summary>
    [JsonPropertyName("g")]
    public string TeamsRole { get; set; }

    /// <summary>Teams註冊名子</summary>
    [JsonPropertyName("h")]
    public string TeamsContacterRegisterLastName { get; set; }

    /// <summary>Teams註冊姓氏</summary>
    [JsonPropertyName("i")]
    public string TeamsContacterRegisterFirstName { get; set; }

    /// <summary>Teams註冊電子郵件</summary>
    [JsonPropertyName("j")]
    public string TeamsContacterRegisterEmail { get; set; }

    /// <summary>Teams註冊時間</summary>
    [JsonPropertyName("k")]
    public DateTimeOffset? TeamsRegistrationTime { get; set; }

    /// <summary>Teams註冊狀態</summary>
    [JsonPropertyName("l")]
    public string TeamsRegistrationStatus { get; set; }

    /// <summary>Teams公司名稱</summary>
    [JsonPropertyName("m")]
    public string TeamsCompanyName { get; set; }

    /// <summary>Teams部門</summary>
    [JsonPropertyName("n")]
    public string TeamsContacterDepartment { get; set; }

    /// <summary>Teams職稱</summary>
    [JsonPropertyName("o")]
    public string TeamsContacterJobTitle { get; set; }

    /// <summary>Teams公司電話</summary>
    [JsonPropertyName("p")]
    public string TeamsCompanyTelephone { get; set; }

    /// <summary>Teams手機號碼</summary>
    [JsonPropertyName("q")]
    public string TeamsContacterPhone { get; set; }

    /// <summary>Teams活動資訊來源</summary>
    [JsonPropertyName("r")]
    public string TeamsActivityInfoSource { get; set; }

    /// <summary>Teams窗口是否個資同意</summary>
    [JsonPropertyName("s")]
    public bool TeamsContacterIsConsent { get; set; }
}
