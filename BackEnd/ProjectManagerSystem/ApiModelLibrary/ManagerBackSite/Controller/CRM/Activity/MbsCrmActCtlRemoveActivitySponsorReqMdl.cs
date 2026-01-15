using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動贊助-控制器-刪除-請求模型</summary>
public class MbsCrmActCtlRemoveActivitySponsorReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動贊助商ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivitySponsorID { get; set; }
}
