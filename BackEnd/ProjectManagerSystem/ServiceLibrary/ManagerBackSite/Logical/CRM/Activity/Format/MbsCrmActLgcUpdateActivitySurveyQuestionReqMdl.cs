using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-更新活動問卷問題-請求模型</summary>
public class MbsCrmActLgcUpdateActivitySurveyQuestionReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷問題列表</summary>
    public List<MbsCrmActLgcUpdateActivitySurveyQuestionReqItemQuestionMdl> ManagerActivitySurveyQuestionList { get; set; }
}