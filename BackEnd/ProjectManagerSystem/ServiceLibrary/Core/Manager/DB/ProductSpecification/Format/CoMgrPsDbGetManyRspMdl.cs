using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆-回應模型</summary>
public class CoMgrPsDbGetManyRspMdl
{
    /// <summary>管理者-產品規格-列表</summary>
    public List<CoMgrPsDbGetManyRspItemMdl> ManagerProductSpecificationList { get; set; }
}