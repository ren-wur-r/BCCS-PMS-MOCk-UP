using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆-回應模型</summary>
public class CoMgrPrdDbGetManyRspMdl
{
    /// <summary>管理者-產品-列表</summary>
    public List<CoMgrPrdDbGetManyRspItemMdl> ManagerProductList { get; set; }
}