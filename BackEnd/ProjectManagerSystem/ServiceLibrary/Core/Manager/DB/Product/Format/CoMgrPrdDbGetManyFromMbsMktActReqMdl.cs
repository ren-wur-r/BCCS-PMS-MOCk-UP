using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-CRM-活動管理]-請求模型</summary>
public class CoMgrPrdDbGetManyFromMbsMktActReqMdl
{
    /// <summary>管理者產品ID列表</summary>
    public List<int> ManagerProductIDList { get; set; }
}