using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得主分類-回應模型</summary>
public class MbsSysPrdLgcGetMainKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-業務毛利率</summary>
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    public bool ManagerProductMainKindIsEnable { get; set; }
}