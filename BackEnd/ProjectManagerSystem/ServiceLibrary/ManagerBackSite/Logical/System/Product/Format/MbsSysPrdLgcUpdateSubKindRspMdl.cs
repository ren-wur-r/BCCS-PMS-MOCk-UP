using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-更新子分類-回應模型</summary>
public class MbsSysPrdLgcUpdateSubKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }
}