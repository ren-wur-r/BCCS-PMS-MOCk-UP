using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

/// <summary>核心-管理者-活動產品-資料庫-取得多筆-回應</summary>
public class CoMgrActPrdDbGetManyRspMdl
{
    /// <summary>管理者活動產品列表</summary>
    public List<CoMgrActPrdDbGetManyRspItemMdl> ManagerActivityProductList { get; set; }
}
