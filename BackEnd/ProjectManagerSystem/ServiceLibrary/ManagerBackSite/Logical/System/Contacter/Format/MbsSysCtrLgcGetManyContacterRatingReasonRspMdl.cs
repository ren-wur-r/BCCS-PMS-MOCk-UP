using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得多筆窗口開發評等原因-回應模型</summary>
public class MbsSysCtrLgcGetManyContacterRatingReasonRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口開發評等原因-列表</summary>
    public List<MbsSysCtrLgcGetManyContacterRatingReasonRspItemMdl> ManagerContacterRatingReasonList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}
