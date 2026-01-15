using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得活動問卷問題-回應模型</summary>
public class MbsCrmActLgcGetActivitySurveyQuestionRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動問卷問題列表</summary>
    public List<MbsCrmActLgcGetActivitySurveyQuestionRspItemMdl> ManagerActivitySurveyQuestionList { get; set; }
}