using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

/// <summary>核心-管理者-活動問卷問題-資料庫-新增多筆-請求模型</summary>
public class CoMgrAsqDbAddManyReqMdl
{
    /// <summary>管理者活動問卷問題列表</summary>
    public List<CoMgrAsqDbAddManyReqItemMdl> ManagerActivitySurveyQuestionList { get; set; }
}