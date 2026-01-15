using System.Collections.Generic;
using DataModelLibrary.Database.ManagerProduct;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-新增-請求模型</summary>
public class MbsSysPrdLgcAddReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-主分類-ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-類型</summary>
    public DbManagerProductKindEnum ManagerProductKind { get; set; }

    /// <summary>管理者-產品-是否主力產品</summary>
    public bool ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-備註</summary>
    public string ManagerProductRemark { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    public bool ManagerProductIsEnable { get; set; }

    /// <summary>管理者-產品-規格列表</summary>
    public List<MbsSysPrdLgcAddSpecificationItemReqMdl> ManagerProductSpecificationList { get; set; }
}