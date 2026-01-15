using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增-請求模型</summary>
public class MbsCrmActCtlAddActivityReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(20, ErrorMessage = "Invalid parameters")]
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動類型</summary>
    [Required]
    [JsonPropertyName("b")]
    public DbAtomManagerActivityKindEnum ManagerAcitivtyKind { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    [Required]
    [JsonPropertyName("c")]
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    [Required]
    [JsonPropertyName("d")]
    public DateTimeOffset ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-地點</summary>
    [Required]
    [JsonPropertyName("e")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerActivityLocation { get; set; }

    /// <summary>管理者活動-期望名單數</summary>
    [Required]
    [JsonPropertyName("f")]
    public int ManagerActivityExpectedLeadCount { get; set; }

    /// <summary>管理者活動-內容</summary>
    [JsonPropertyName("g")]
    [StringLength(2000, ErrorMessage = "Invalid parameters")]
    public string ManagerActivityContent { get; set; }

    /// <summary>管理者活動-活動產品清單</summary>
    [JsonPropertyName("h")]
    public List<MbsCrmActCtlAddActivityReqProductItemMdl> ManagerActivityProductList { get; set; }

    /// <summary>管理者活動-活動贊助清單</summary>
    [JsonPropertyName("i")]
    public List<MbsCrmActCtlAddActivityReqSponsorItemMdl> ManagerActivitySponsorList { get; set; }

    /// <summary>管理者活動-活動支出清單</summary>
    [JsonPropertyName("j")]
    public List<MbsCrmActCtlAddActivityReqExpenseItemMdl> ManagerActivityExpenseList { get; set; }
}
