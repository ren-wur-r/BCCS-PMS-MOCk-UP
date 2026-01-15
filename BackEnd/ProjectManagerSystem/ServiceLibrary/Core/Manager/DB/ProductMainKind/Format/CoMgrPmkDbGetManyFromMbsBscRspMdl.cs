using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrPmkDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者-產品主分類-列表</summary>
    public List<CoMgrPmkDbGetManyFromMbsBscRspItemMdl> ManagerProductMainKindList { get; set; }
}