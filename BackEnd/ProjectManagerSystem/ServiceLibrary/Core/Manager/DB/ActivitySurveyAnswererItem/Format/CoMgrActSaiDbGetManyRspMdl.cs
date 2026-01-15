using System.Collections.Generic;
namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Format;

/// <summary>核心-管理者-活動問卷回答項目-資料庫-取得多筆-回應模型</summary>
public class CoMgrActSaiDbGetManyRspMdl
{
    /// <summary>管理者活動問卷回答項目清單</summary>
    public List<CoMgrActSaiDbGetManyRspItemMdl> ManagerActivitySurveyAnswererItemList { get; set; }
}