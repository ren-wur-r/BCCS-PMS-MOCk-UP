using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增活動問卷問題-回應模型</summary>
public class MbsCrmActCtlAddActivitySurveyQuestionRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-活動問卷問題-ID列表</summary>
    [JsonPropertyName("a")]
    public List<int> ManagerActivitySurveyQuestionIDList { get; set; }
}