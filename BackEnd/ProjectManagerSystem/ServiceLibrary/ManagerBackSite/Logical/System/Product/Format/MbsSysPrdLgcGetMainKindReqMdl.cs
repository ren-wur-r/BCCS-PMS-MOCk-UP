using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得主分類-請求模型</summary>
public class MbsSysPrdLgcGetMainKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    public int ManagerProductMainKindID { get; set; }
}