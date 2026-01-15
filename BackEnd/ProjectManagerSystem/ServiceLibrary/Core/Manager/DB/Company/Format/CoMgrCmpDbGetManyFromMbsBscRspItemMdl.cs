namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-基本]-項目-回應模型</summary>
public class CoMgrCmpDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者公司-編號</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司-名稱</summary>
    public string ManagerCompanyName { get; set; }
}