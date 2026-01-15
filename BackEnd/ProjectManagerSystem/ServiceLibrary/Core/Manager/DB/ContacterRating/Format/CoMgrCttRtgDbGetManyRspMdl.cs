using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRating.Format;

/// <summary>核心-管理者-窗口開發評等-資料庫-取得多筆-回應模型</summary>
public class CoMgrCttRtgDbGetManyRspMdl
{
    /// <summary>管理者窗口開發評等-列表</summary>
    public List<CoMgrCttRtgDbGetManyRspItemMdl> ManagerContacterRatingList { get; set; }
}
