using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得活動問卷問題-請求模型</summary>
public class MbsCrmActCtlGetActivitySurveyQuestionReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerActivityID { get; set; }
}