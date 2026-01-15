using DataModelLibrary.Database.ManagerActivitySurveyQuestion;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

/// <summary>管理者-活動問卷問題-資料庫-新增-請求模型</summary>
public class CoMgrAsqDbAddReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷問題-類型</summary>
    public DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者活動問卷問題-問題標題</summary>
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者活動問卷問題-問題說明</summary>
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>管理者活動問卷問題-新增其他選項</summary>
    public bool IsOther { get; set; }

    /// <summary>管理者活動問卷問題-排序</summary>
    public short ManagerActivitySurveyQuestionSort { get; set; }

}
