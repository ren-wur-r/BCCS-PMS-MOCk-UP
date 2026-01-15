namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆[名稱]-項目-回應模型</summary>
public class CoMgrCmpDbGetManyNameRspItemMdl
{
    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司名稱</summary>
    public string ManagerCompanyName { get; set; }
}