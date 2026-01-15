using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動贊助-控制器-取得多筆-回應模型</summary>
public class MbsCrmActCtlGetManyActivitySponsorRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動贊助商列表</summary>
    [Required]
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlGetManyActivitySponsorRspItemMdl> ManagerActivitySponsorList { get; set; }
}
