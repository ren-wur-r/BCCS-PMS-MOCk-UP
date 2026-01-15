using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-基本]</summary>
public class CoMgrPrdDbGetManyFromMbsBscRspMdl
{
    /// <summary>產品列表</summary>
    public List<CoMgrPrdDbGetManyFromMbsBscRspItemMdl> ManagerProductList { get; set; }
}