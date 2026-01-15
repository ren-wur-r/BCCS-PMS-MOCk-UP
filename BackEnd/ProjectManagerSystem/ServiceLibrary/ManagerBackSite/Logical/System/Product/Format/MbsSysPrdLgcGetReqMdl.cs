using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得-請求模型</summary>
public class MbsSysPrdLgcGetReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }
}