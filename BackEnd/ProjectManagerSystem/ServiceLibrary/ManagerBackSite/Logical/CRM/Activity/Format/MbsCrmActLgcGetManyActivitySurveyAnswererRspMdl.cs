using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動問卷回答者-回應模型</summary>
public class MbsCrmActLgcGetManyActivitySurveyAnswererRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動問卷回答者列表</summary>
    public List<MbsCrmActLgcGetManyActivitySurveyAnswererRspItemMdl> ManagerActivitySurveyAnswererList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}