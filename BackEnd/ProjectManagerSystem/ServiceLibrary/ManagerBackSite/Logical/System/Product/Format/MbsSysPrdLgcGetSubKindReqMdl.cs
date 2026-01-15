using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得子分類-請求模型</summary>
public class MbsSysPrdLgcGetSubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }
}