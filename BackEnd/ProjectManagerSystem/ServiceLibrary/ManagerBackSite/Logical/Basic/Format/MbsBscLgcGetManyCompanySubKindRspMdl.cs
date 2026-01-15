using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司子分類-回應模型</summary>
public class MbsBscLgcGetManyCompanySubKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>公司子分類列表</summary>
    public List<MbsBscLgcGetManyCompanySubKindRspItemMdl> ManagerCompanySubKindList { get; set; }
}
