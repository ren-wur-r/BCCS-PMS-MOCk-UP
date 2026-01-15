using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆-回應模型</summary>
public class CoMgrPmkDbGetManyRspMdl
{
    /// <summary>管理者-產品主分類-列表</summary>
    public List<CoMgrPmkDbGetManyRspItemMdl> ManagerProductMainKindList { get; set; }
}