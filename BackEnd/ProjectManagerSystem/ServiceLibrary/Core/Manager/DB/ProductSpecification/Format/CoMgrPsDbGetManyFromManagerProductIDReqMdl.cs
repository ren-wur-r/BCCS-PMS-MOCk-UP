using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆從[產品ID]-請求模型</summary>
public class CoMgrPsDbGetManyFromManagerProductIDReqMdl
{
    /// <summary>產品ID清單</summary>
    public List<int> ManagerProductIDList { get; set; }

    /// <summary>產品規格是否啟用</summary>
    public bool? ManagerProductSpecificationIsEnable { get; set; }
}
