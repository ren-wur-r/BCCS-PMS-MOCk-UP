using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增活動問卷問題-請求模型</summary>
public class MbsCrmActCtlAddActivitySurveyQuestionReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-活動問卷問題列表</summary>
    [JsonPropertyName("a")]
    [Required]
    public List<MbsCrmActCtlAddActivitySurveyQuestionReqItemQuestionMdl> ManagerActivitySurveyQuestionList { get; set; }
}