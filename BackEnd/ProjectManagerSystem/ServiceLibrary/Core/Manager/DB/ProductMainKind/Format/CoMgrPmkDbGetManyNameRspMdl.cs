using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrPmkDbGetManyNameRspMdl
{
    /// <summary>產品主分類列表</summary>
    public List<CoMgrPmkDbGetManyNameRspItemMdl> ManagerProductMainKindList { get; set; }
}