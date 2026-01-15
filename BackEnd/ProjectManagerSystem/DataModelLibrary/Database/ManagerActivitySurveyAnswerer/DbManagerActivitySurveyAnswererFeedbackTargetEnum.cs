using System;

namespace DataModelLibrary.Database.ManagerActivitySurveyAnswerer;

/// <summary>管理者活動問卷-回饋目標</summary>
[Flags]
public enum DbManagerActivitySurveyAnswererFeedbackTargetEnum : short
{
    /// <summary>無</summary>
    None = 0,

    /// <summary>了解產品</summary>
    LearnProduct = 1,

    /// <summary>對產品/服務感興趣</summary>
    InterestedInProduct = 2,

    /// <summary>對活動禮感興趣</summary>
    InterestedInGift = 4,

    /// <summary>評估/尋找解決方案</summary>
    EvaluatingSolution = 8,
}
