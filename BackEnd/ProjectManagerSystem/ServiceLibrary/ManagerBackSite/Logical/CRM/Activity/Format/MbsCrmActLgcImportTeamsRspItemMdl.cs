using System;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-匯入Teams-回應項目模型</summary>
public class MbsCrmActLgcImportTeamsRspItemMdl
{
    /// <summary>Teams名稱</summary>
    public string TeamsName { get; set; }

    /// <summary>Teams首次加入時間</summary>
    public DateTimeOffset? TeamsFirstJoinTime { get; set; }

    /// <summary>Teams上次離開時間</summary>
    public DateTimeOffset? TeamsLastLeaveTime { get; set; }

    /// <summary>Teams會議持續時間</summary>
    public string TeamsMeetingDuration { get; set; }

    /// <summary>Teams電子郵件</summary>
    public string TeamsEmail { get; set; }

    /// <summary>參與者識別碼 (UPN)</summary>
    public string TeamsParticipantId { get; set; }

    /// <summary>Teams角色</summary>
    public string TeamsRole { get; set; }

    /// <summary>Teams註冊名子</summary>
    public string TeamsContacterRegisterLastName { get; set; }

    /// <summary>Teams註冊姓氏</summary>
    public string TeamsContacterRegisterFirstName { get; set; }

    /// <summary>Teams註冊電子郵件</summary>
    public string TeamsContacterRegisterEmail { get; set; }

    /// <summary>Teams註冊時間</summary>
    public DateTimeOffset? TeamsRegistrationTime { get; set; }

    /// <summary>Teams註冊狀態</summary>
    public string TeamsRegistrationStatus { get; set; }

    /// <summary>Teams公司名稱</summary>
    public string TeamsCompanyName { get; set; }

    /// <summary>Teams部門</summary>
    public string TeamsContacterDepartment { get; set; }

    /// <summary>Teams職稱</summary>
    public string TeamsContacterJobTitle { get; set; }

    /// <summary>Teams公司電話</summary>
    public string TeamsCompanyTelephone { get; set; }

    /// <summary>Teams手機號碼</summary>
    public string TeamsContacterPhone { get; set; }

    /// <summary>Teams活動資訊來源</summary>
    public string TeamsActivityInfoSource { get; set; }

    /// <summary>Teams窗口是否個資同意</summary>
    public bool TeamsContacterIsConsent { get; set; }
}