using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-批次新增活動問卷問題-請求模型</summary>
public class MbsCrmActLgcAddActivitySurveyQuestionReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動問卷問題列表</summary>
    public List<MbsCrmActLgcAddActivitySurveyQuestionReqItemQuestionMdl> ManagerActivitySurveyQuestionList { get; set; }
}