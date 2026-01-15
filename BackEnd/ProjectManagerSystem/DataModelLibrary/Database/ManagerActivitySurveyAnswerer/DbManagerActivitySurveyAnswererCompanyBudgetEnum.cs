namespace DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

/// <summary>管理者活動問卷-公司預算</summary>
public enum DbManagerActivitySurveyAnswererCompanyBudgetEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>50萬以內</summary>
    Under500k = 1,

    /// <summary>50~100萬</summary>
    From500kTo1M = 2,

    /// <summary>100~300萬</summary>
    From1MTo3M = 3,

    /// <summary>500萬以上</summary>
    Over5M = 4,

    /// <summary>不清楚</summary>
    Unknown = 5,
}
