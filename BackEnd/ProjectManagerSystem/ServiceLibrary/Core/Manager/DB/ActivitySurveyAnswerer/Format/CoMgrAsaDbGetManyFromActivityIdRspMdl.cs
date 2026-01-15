using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Format;

/// <summary>核心-管理者-活動問卷回答者-資料庫-取得多筆-回應模型</summary>
public class CoMgrAsaDbGetManyFromActivityIdRspMdl
{
    /// <summary>管理者活動問卷回答者資料庫-取得多筆-回應項目模型</summary>
    public List<CoMgrAsaDbGetManyFromActivityIdRspItemMdl> ManagerActivitySurveyAnswererList { get; set; }
}