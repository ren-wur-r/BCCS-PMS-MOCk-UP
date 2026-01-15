using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆窗口開發評等原因-回應模型</summary>
public class MbsBscLgcGetManyContacterRatingReasonRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>窗口開發評等原因列表</summary>
    public List<MbsBscLgcGetManyContacterRatingReasonRspItemMdl> ManagerContacterRatingReasonList { get; set; }
}
