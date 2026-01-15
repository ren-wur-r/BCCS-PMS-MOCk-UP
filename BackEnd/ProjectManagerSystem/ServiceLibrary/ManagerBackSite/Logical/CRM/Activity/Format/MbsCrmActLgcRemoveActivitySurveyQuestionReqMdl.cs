using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-刪除活動問卷問題-請求模型</summary>
public class MbsCrmActLgcRemoveActivitySurveyQuestionReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動問卷問題ID</summary>
    public int ManagerActivitySurveyQuestionID { get; set; }
}