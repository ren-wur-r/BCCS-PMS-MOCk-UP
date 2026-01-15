using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrPskDbGetManyNameReqMdl
{
    /// <summary>產品子分類ID列表</summary>
    public List<int> ManagerProductSubKindIDList { get; set; }
}