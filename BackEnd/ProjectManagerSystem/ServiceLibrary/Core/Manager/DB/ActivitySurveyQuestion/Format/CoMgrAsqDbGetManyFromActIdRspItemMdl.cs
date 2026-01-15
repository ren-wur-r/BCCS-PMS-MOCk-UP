using DataModelLibrary.Database.ManagerActivitySurveyQuestion;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

/// <summary>核心-管理者-活動問卷問題-資料庫-取得多筆從[管理者活動ID]-回應項目模型</summary>
public class CoMgrAsqDbGetManyFromActIdRspItemMdl
{
    /// <summary>管理者活動問卷問題ID</summary>
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>管理者活動問卷問題類型</summary>
    public DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者活動問卷問題標題</summary>
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者活動問卷問題說明</summary>
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>管理者活動問卷問題新增其他選項</summary>
    public bool IsOther { get; set; }

    /// <summary>管理者活動問卷問題排序</summary>
    public short ManagerActivitySurveyQuestionSort { get; set; }
}