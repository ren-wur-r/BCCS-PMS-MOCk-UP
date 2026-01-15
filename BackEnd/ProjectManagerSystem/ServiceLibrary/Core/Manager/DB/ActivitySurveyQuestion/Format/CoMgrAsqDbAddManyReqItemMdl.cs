using DataModelLibrary.Database.ManagerActivitySurveyQuestion;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

/// <summary>核心-管理者-活動問卷問題-資料庫-新增多筆-請求項目模型</summary>
public class CoMgrAsqDbAddManyReqItemMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷問題類型</summary>
    public DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者活動問卷問題標題</summary>
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者活動問卷問題內容</summary>
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>管理者活動問卷問題是否為其他</summary>
    public bool IsOther { get; set; }

    /// <summary>管理者活動問卷問題排序</summary>
    public short ManagerActivitySurveyQuestionSort { get; set; }
}