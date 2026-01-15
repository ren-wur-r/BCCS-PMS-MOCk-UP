using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動產品-控制器-取得多筆-請求模型</summary>
public class MbsCrmActCtlGetManyActivityProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityID { get; set; }
}
