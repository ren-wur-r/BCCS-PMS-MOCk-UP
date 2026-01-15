using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆產品-回應模型</summary>
public class MbsBscLgcGetManyProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>產品列表</summary>
    public List<MbsBscLgcGetManyProductRspItemMdl> ManagerProductList { get; set; }
}
