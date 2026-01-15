using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆-回應模型</summary>
public class CoMgrCttRtgRsnDbGetManyRspMdl
{
    /// <summary>管理者窗口開發評等原因-列表</summary>
    public List<CoMgrCttRtgRsnDbGetManyRspItemMdl> ManagerContacterRatingReasonList { get; set; }
}
