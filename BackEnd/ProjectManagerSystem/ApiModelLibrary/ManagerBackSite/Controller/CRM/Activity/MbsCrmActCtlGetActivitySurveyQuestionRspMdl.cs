using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得活動問卷問題-回應模型</summary>
public class MbsCrmActCtlGetActivitySurveyQuestionRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動問卷問題列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlGetActivitySurveyQuestionRspItemMdl> ManagerActivitySurveyQuestionList { get; set; }
}