using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-系統設定-產品]-回應模型</summary>
public class CoMgrPrdDbGetManyFromMbsSysPrdRspMdl
{
    /// <summary>管理者-產品-列表</summary>
    public List<CoMgrPrdDbGetManyFromMbsSysPrdRspItemMdl> ManagerProductList { get; set; }
}