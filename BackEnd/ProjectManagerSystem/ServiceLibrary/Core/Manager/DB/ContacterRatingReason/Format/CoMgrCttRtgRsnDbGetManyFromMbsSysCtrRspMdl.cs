using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得[多筆]從[管理者後台-客戶名單-客戶窗口]-回應模型</summary>
public class CoMgrCttRtgRsnDbGetManyFromMbsSysCtrRspMdl
{
    /// <summary>管理者窗口開發評等原因-列表</summary>
    public List<CoMgrCttRtgRsnDbGetManyFrommbsSysCtrRspItemMdl> ManagerContacterRatingReasonList { get; set; }
}