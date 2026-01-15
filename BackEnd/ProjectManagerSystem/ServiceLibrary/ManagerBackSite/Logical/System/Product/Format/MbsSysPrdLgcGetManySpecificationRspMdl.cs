using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得多筆規格-回應模型</summary>
public class MbsSysPrdLgcGetManySpecificationRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-產品規格-列表</summary>
    public List<MbsSysPrdLgcGetManySpecificationRspItemMdl> ManagerProductSpecificationList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}