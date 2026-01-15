namespace DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

/// <summary>管理者活動問卷-公司規模</summary>
public enum DbManagerActivitySurveyAnswererCompanyScaleEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,


    /// <summary>少於100人</summary>
    LessThan100 = 1,

    /// <summary>100~500人</summary>
    From100To500 = 2,

    /// <summary>500~1000人</summary>
    From500To1000 = 3,

    /// <summary>1000人以上</summary>
    MoreThan1000 = 4,

    /// <summary>不清楚</summary>
    Unknown = 5,
}
