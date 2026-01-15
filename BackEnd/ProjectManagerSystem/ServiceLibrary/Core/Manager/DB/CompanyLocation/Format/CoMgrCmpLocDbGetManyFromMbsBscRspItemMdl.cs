namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-取得多筆從[管理者後台-基本]-項目-回應模型</summary>
public class CoMgrCmpLocDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者公司營業地點ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點名稱</summary>
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>管理者公司營業地點-是否已刪除</summary>
    public bool ManagerCompanyLocationIsDeleted { get; set; }
}