using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆-請求模型</summary>
public class MbsCrmActCtlGetManyActivityReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動-開始時間</summary>
    [JsonPropertyName("a")]
    public DateTimeOffset? ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset? ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-類型</summary>
    [JsonPropertyName("c")]
    public DbAtomManagerActivityKindEnum? ManagerActivityKind { get; set; }

    /// <summary>管理者活動-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerActivityName { get; set; }

    /// <summary>頁數</summary>
    [Required]
    [JsonPropertyName("e")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [Required]
    [JsonPropertyName("f")]
    public int PageSize { get; set; }
}
