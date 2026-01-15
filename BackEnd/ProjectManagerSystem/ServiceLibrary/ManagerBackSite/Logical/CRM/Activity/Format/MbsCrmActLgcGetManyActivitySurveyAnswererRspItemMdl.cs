using DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動問卷回答者-回應項目模型</summary>
public class MbsCrmActLgcGetManyActivitySurveyAnswererRspItemMdl
{
    /// <summary>管理者活動問卷回答者-ID</summary>
    public int ManagerActivitySurveyAnswererID { get; set; }

    /// <summary>管理者活動問卷回答者-公司名稱</summary>
    public string ManagerActivitySurveyAnswererCompanyName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口姓名</summary>
    public string ManagerActivitySurveyAnswererContacterName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口信箱</summary>
    public string ManagerActivitySurveyAnswererContacterEmail { get; set; }

    /// <summary>管理者活動問卷回答者-公司規模</summary>
    public DbManagerActivitySurveyAnswererCompanyScaleEnum ManagerActivitySurveyAnswererCompanyScaleID { get; set; }

    /// <summary>管理者活動問卷回答者-公司預算</summary>
    public DbManagerActivitySurveyAnswererCompanyBudgetEnum ManagerActivitySurveyAnswererCompanyBudgetID { get; set; }

    /// <summary>管理者活動問卷回答者-公司採購</summary>
    public DbManagerActivitySurveyAnswererCompanyPurchaseEnum ManagerActivitySurveyAnswererCompanyPurchaseID { get; set; }
}