using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-是否存在多筆</summary>
public class CoMgrCttRtgRsnDbExistManyReqMdl
{
    /// <summary>管理者窗口開發評等原因ID列表</summary>
    public List<int> ManagerContacterRatingReasonIDList { get; set; }
}