using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-更新-請求模型</summary>
public class MbsCrmActCtlUpdateActivityReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動名稱</summary>
    [JsonPropertyName("b")]
    [StringLength(20, ErrorMessage = "Invalid parameters")]
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset? ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset? ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-地點</summary>
    [JsonPropertyName("e")]
    [StringLength(50, ErrorMessage = "地點長度不可超過50個字")]
    public string ManagerActivityLocation { get; set; }

    /// <summary>管理者活動-期望名單數</summary>
    [JsonPropertyName("f")]
    public int? ManagerActivityExpectedLeadCount { get; set; }

    /// <summary>管理者活動-內容</summary>
    [JsonPropertyName("g")]
    [StringLength(2000, ErrorMessage = "內容長度不可超過2000個字")]
    public string ManagerActivityContent { get; set; }
}
