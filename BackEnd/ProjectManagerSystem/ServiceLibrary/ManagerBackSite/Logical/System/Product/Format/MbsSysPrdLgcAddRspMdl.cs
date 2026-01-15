using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-新增-回應模型</summary>
public class MbsSysPrdLgcAddRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>產品ID</summary>
    public int ManagerProductID { get; set; }
}