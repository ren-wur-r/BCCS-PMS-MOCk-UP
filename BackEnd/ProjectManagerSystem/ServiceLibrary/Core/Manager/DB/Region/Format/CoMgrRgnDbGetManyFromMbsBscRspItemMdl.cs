namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoMgrRgnDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>地區-是否啟用</summary>
    public bool ManagerRegionIsEnable { get; set; }
}