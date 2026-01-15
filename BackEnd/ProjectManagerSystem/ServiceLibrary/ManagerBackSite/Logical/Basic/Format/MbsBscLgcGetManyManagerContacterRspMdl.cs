using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆窗口-回應模型</summary>
public class MbsBscLgcGetManyManagerContacterRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口列表</summary>
    public List<MbsBscLgcGetManyContacterRspItemMdl> ManagerContacterList { get; set; }
}
