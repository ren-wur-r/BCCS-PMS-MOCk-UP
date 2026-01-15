using DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Format;

/// <summary>核心-管理者-活動問卷回答者-資料庫-取得-回應模型</summary>
public class CoMgrAsaDbGetRspMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷-是否同意</summary>
    public bool ManagerActivitySurveyAnswererIsConsent { get; set; }

    /// <summary>管理者活動問卷-公司名稱</summary>
    public string ManagerActivitySurveyAnswererCompanyName { get; set; }

    /// <summary>管理者活動問卷-公司電話</summary>
    public string ManagerActivitySurveyAnswererCompanyPhone { get; set; }

    /// <summary>管理者活動問卷-公司地址</summary>
    public string ManagerActivitySurveyAnswererCompanyAddress { get; set; }

    /// <summary>管理者活動問卷-窗口名稱</summary>
    public string ManagerActivitySurveyAnswererContacterName { get; set; }

    /// <summary>管理者活動問卷-窗口信箱</summary>
    public string ManagerActivitySurveyAnswererContacterEmail { get; set; }

    /// <summary>管理者活動問卷-窗口手機</summary>
    public string ManagerActivitySurveyAnswererContacterPhone { get; set; }

    /// <summary>管理者活動問卷-窗口部門</summary>
    public string ManagerActivitySurveyAnswererContacterDepartment { get; set; }

    /// <summary>管理者活動問卷-窗口職稱</summary>
    public string ManagerActivitySurveyAnswererContacterJobTitle { get; set; }

    /// <summary>管理者活動問卷-窗口電話</summary>
    public string ManagerActivitySurveyAnswererContacterTelephone { get; set; }

    /// <summary>管理者活動問卷-公司規模ID</summary>
    public DbManagerActivitySurveyAnswererCompanyScaleEnum ManagerActivitySurveyAnswererCompanyScaleID { get; set; }

    /// <summary>管理者活動問卷-公司預算ID</summary>
    public DbManagerActivitySurveyAnswererCompanyBudgetEnum ManagerActivitySurveyAnswererCompanyBudgetID { get; set; }

    /// <summary>管理者活動問卷-公司採購ID</summary>
    public DbManagerActivitySurveyAnswererCompanyPurchaseEnum ManagerActivitySurveyAnswererCompanyPurchaseID { get; set; }

    /// <summary>管理者活動問卷-回饋目標ID</summary>
    public DbManagerActivitySurveyAnswererFeedbackTargetEnum ManagerActivitySurveyAnswererFeedbackTargetID { get; set; }

    /// <summary>管理者活動問卷-回饋時程ID</summary>
    public DbManagerActivitySurveyAnswererFeedbackScheduleEnum ManagerActivitySurveyAnswererFeedbackScheduleID { get; set; }

}