namespace DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

/// <summary>管理者活動問卷-回饋時程</summary>
public enum DbManagerActivitySurveyAnswererFeedbackScheduleEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>暫無計畫</summary>
    NoPlan = 1,

    /// <summary>半年後</summary>
    AfterSixMonths = 2,

    /// <summary>未來一年內</summary>
    WithinOneYear = 3,

    /// <summary>未來兩年內</summary>
    WithinTwoYears = 4,

}
