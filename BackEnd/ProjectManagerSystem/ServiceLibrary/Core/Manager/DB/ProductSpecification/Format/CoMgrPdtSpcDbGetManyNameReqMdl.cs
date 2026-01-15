using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆名稱-請求模型</summary>
public class CoMgrPdtSpcDbGetManyNameReqMdl
{
    /// <summary>管理者-產品規格-ID列表</summary>
    public List<int> ManagerProductSpecificationIDList { get; set; }
}
