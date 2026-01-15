using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrPrdDbGetManyNameRspMdl
{
    /// <summary>產品清單</summary>
    public List<CoMgrPrdDbGetManyNameRspItemMdl> ManagerProductList { get; set; }
}