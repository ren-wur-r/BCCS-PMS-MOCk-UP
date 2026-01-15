using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrCttRtgRsnDbGetManyNameRspMdl
{
    /// <summary>窗口開發評等原因項目列表</summary>
    public List<CoMgrCttRtgRsnDbGetManyNameRspItemMdl> ManagerContacterRatingReasonList { get; set; }
}