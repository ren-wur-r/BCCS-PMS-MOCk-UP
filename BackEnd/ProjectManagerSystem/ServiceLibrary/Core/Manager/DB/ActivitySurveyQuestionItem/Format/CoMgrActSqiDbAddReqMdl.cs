namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;
public class CoMgrActSqiDbAddReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷問題-ID</summary>
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>管理者活動問卷問題項目-名稱</summary>
    public string ManagerActivitySurveyQuestionItemName { get; set; }

    /// <summary>管理者活動問卷問題項目-排序</summary>
    public short ManagerActivitySurveyQuestionItemSort { get; set; }
}