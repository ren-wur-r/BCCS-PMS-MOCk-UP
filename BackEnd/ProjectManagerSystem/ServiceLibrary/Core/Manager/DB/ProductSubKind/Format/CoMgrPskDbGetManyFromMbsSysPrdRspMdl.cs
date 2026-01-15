using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-系統設定-產品[子分類]]-回應模型</summary>
public class CoMgrPskDbGetManyFromMbsSysPrdRspMdl
{
    /// <summary>管理者-產品子分類-列表</summary>
    public List<CoMgrPskDbGetManyFromMbsSysPrdRspItemMdl> ManagerProductSubKindList { get; set; }
}