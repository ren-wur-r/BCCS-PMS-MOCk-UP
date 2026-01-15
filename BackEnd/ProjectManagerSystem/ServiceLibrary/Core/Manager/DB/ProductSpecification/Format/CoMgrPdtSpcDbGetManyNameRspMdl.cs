using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆名稱-回應模型</summary>
public class CoMgrPdtSpcDbGetManyNameRspMdl
{
    /// <summary>管理者-產品規格列表</summary>
    public List<CoMgrPdtSpcDbGetManyNameRspItemMdl> ManagerProductSpecificationList { get; set; }
}
