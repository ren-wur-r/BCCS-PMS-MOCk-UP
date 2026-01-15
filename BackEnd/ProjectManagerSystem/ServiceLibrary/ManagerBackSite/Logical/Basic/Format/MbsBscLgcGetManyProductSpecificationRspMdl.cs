using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆產品規格-回應模型</summary>
public class MbsBscLgcGetManyProductSpecificationRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>產品規格列表</summary>
    public List<MbsBscLgcGetManyProductSpecificationRspItemMdl> ManagerProductSpecificationList { get; set; }
}