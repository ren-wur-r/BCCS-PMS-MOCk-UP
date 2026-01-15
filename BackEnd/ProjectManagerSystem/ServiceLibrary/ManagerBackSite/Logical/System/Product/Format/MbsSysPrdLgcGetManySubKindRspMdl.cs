using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得多筆子分類-回應模型</summary>
public class MbsSysPrdLgcGetManySubKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-產品子分類-列表</summary>
    public List<MbsSysPrdLgcGetManySubKindRspItemMdl> ManagerProductSubKindList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}