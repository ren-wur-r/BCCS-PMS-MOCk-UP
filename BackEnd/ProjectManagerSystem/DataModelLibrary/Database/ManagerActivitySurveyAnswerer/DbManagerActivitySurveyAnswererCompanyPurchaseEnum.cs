namespace DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

/// <summary>管理者活動問卷-採購</summary>
public enum DbManagerActivitySurveyAnswererCompanyPurchaseEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>負責評估產品</summary>
    Evaluator = 1,

    /// <summary>不直接負責但可以提供建議</summary>
    Advisor = 2,

    /// <summary>負責決定試用解決方案/產品</summary>
    Decision = 3,

    /// <summary>無特別角色</summary>
    None = 4,
}
