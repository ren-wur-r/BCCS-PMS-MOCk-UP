using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動問卷問題-回應模型</summary>
public class MbsCrmActLgcAddActivitySurveyQuestionRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動問卷問題ID列表</summary>
    public List<int> ManagerActivitySurveyQuestionIDList { get; set; }
}