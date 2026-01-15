namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoMgrCmpSubKdDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者-公司子分類-ID</summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者-公司子分類-名稱</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    public bool ManagerCompanySubKindIsEnable { get; set; }
}