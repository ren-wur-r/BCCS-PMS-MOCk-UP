using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得活動問卷問題-請求模型</summary>
public class MbsCrmActLgcGetActivitySurveyQuestionReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }
}