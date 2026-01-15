using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrCttRtgRsnDbGetManyNameReqMdl
{
    /// <summary>窗口開發評等原因ID列表</summary>
    public List<int> ManagerContacterRatingReasonIDList { get; set; }
}