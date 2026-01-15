using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得規格-請求模型</summary>
public class MbsSysPrdLgcGetSpecificationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品規格-ID</summary>
    public int ManagerProductSpecificationID { get; set; }
}