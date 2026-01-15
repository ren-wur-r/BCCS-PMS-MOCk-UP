using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-CRM-活動管理]-回應模型</summary>
public class CoMgrPrdDbGetManyFromMbsMktActRspMdl
{
    /// <summary>管理者產品列表</summary>
    public List<CoMgrPrdDbGetManyFromMbsMktActRspItemMdl> ManagerProductList { get; set; }
}