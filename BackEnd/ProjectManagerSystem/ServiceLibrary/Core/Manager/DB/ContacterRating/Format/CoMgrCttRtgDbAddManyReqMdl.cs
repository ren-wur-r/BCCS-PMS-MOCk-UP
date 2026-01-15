using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ContacterRating.Format;

/// <summary>核心-管理者-窗口開發評等-資料庫-新增多筆-請求模型</summary>
public class CoMgrCttRtgDbAddManyReqMdl
{
    /// <summary>窗口開發評等列表</summary>
    public List<CoMgrCttRtgDbAddManyReqItemMdl> ManagerContacterRatingList { get; set; }
}
