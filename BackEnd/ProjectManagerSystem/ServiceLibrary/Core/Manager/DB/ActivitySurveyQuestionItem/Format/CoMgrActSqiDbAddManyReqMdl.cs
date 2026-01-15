using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;

/// <summary>核心-管理者-活動問卷問題項目-資料庫-新增多筆-請求</summary>
public class CoMgrActSqiDbAddManyReqMdl
{
    /// <summary>管理者活動問卷問題項目列表</summary>
    public List<CoMgrActSqiDbAddManyReqItemMdl> ManagerActivitySurveyQuestionItemList { get; set; }
}