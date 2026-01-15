namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司營業地點-項目-回應模型</summary>
public class MbsBscLgcGetManyCompanyLocationRspItemMdl
{
    /// <summary>管理者公司營業地點ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點名稱</summary>
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>管理者公司營業地點是否已刪除</summary>
    public bool ManagerCompanyLocationIsDeleted { get; set; }
}