using System.Text.Json.Serialization;
using DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆活動問卷回答者-回應項目模型</summary>
public class MbsCrmActCtlGetManyActivitySurveyAnswererRspItemMdl
{
    /// <summary>管理者活動問卷回答者-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivitySurveyAnswererID { get; set; }

    /// <summary>管理者活動問卷回答者-公司名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerActivitySurveyAnswererCompanyName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerActivitySurveyAnswererContacterName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口信箱</summary>
    [JsonPropertyName("d")]
    public string ManagerActivitySurveyAnswererContacterEmail { get; set; }

    /// <summary>管理者活動問卷回答者-公司規模</summary>
    [JsonPropertyName("e")]
    public DbManagerActivitySurveyAnswererCompanyScaleEnum ManagerActivitySurveyAnswererCompanyScaleID { get; set; }

    /// <summary>管理者活動問卷回答者-公司預算</summary>
    [JsonPropertyName("f")]
    public DbManagerActivitySurveyAnswererCompanyBudgetEnum ManagerActivitySurveyAnswererCompanyBudgetID { get; set; }

    /// <summary>管理者活動問卷回答者-公司採購</summary>
    [JsonPropertyName("g")]
    public DbManagerActivitySurveyAnswererCompanyPurchaseEnum ManagerActivitySurveyAnswererCompanyPurchaseID { get; set; }
}