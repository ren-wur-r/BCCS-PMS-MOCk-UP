using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-刪除活動問卷問題-控制器-請求模型</summary>
public class MbsCrmActCtlRemoveActivitySurveyQuestionReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動問卷問題ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivitySurveyQuestionID { get; set; }
}