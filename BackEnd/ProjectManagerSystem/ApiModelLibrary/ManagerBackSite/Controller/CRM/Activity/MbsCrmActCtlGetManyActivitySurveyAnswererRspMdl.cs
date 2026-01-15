using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆活動問卷回答者-回應模型</summary>
public class MbsCrmActCtlGetManyActivitySurveyAnswererRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動問卷回答者列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlGetManyActivitySurveyAnswererRspItemMdl> ManagerActivitySurveyAnswererList { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("b")]
    public int TotalCount { get; set; }
}