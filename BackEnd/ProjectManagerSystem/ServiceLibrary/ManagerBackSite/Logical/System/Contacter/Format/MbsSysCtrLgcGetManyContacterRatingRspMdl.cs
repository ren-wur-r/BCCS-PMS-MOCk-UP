using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得多筆窗口開發評等-回應模型</summary>
public class MbsSysCtrLgcGetManyContacterRatingRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口開發評等-列表</summary>
    public List<MbsSysCtrLgcGetManyContacterRatingRspItemMdl> ManagerContacterRatingList { get; set; }
}
