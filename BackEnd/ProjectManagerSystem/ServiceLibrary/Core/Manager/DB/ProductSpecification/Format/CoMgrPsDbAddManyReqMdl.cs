using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-新增多筆-請求模型</summary>
public class CoMgrPsDbAddManyReqMdl
{
    /// <summary>管理者-產品規格列表</summary>
    public List<CoMgrPsDbAddManyReqItemMdl> ManagerProductSpecificationList { get; set; }
}