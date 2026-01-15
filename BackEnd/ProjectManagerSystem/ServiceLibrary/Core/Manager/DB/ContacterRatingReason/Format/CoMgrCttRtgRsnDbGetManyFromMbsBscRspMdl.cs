using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrCttRtgRsnDbGetManyFromMbsBscRspMdl
{
    /// <summary>窗口開發評等原因列表</summary>
    public List<CoMgrCttRtgRsnDbGetManyFromMbsBscRspItemMdl> ManagerContacterRatingReasonList { get; set; }
}