using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrPskDbGetManyNameRspMdl
{
    /// <summary>產品子分類名稱列表</summary>
    public List<CoMgrPskDbGetManyNameRspItemMdl> ManagerProductSubKindList { get; set; }
}