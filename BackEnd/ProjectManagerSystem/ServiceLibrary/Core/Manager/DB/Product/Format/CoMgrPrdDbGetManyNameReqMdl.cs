using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrPrdDbGetManyNameReqMdl
{
    /// <summary>產品ID清單</summary>
    public List<int> ManagerProductIdList { get; set; }
}