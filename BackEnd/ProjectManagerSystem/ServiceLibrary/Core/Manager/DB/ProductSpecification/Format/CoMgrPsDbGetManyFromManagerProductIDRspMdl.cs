using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆從[產品ID]-回應模型</summary>
public class CoMgrPsDbGetManyFromManagerProductIDRspMdl
{
    /// <summary>產品規格清單</summary>
    public List<CoMgrPsDbGetManyFromManagerProductIDRspItemMdl> ManagerProductSpecificationList { get; set; }
}
