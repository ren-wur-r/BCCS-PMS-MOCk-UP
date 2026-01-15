using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

/// <summary>核心-管理者-活動產品-資料庫-新增多筆-請求</summary>
public class CoMgrActPrdDbAddManyReqMdl
{
    /// <summary>管理者活動產品列表</summary>
    public List<CoMgrActPrdDbAddManyReqItemMdl> ManagerActivityProductList { get; set; }
}
