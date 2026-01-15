namespace DataModelLibrary.Database.ManagerActivitySurveyQuestion;

/// <summary>管理者活動問卷問題-類型</summary>
public enum DbManagerActivitySurveyQuestionKindEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>單選</summary>
    Single = 1,

    /// <summary>多選</summary>
    Multiple = 2,

    /// <summary>文字</summary>
    Text = 3,

    /// <summary>評分</summary>
    Rating = 4,
}
