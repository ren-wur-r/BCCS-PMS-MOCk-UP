using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-基本-產品規格]-回應模型</summary>
public class CoMgrPsDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者-產品規格-列表</summary>
    public List<CoMgrPsDbGetManyFromMbsBscRspItemMdl> ManagerProductSpecificationList { get; set; }
}