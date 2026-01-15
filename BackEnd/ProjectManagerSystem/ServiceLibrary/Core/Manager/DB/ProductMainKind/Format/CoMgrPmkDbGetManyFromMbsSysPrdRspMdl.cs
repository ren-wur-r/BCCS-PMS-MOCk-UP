using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-系統設定-產品[主分類]]-回應模型</summary>
public class CoMgrPmkDbGetManyFromMbsSysPrdRspMdl
{
    /// <summary>管理者-產品主分類-列表</summary>
    public List<CoMgrPmkDbGetManyFromMbsSysPrdRspItemMdl> ManagerProductMainKindList { get; set; }
}
