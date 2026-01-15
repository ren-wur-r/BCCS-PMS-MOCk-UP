namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者區域-取得多筆-回應項目模型</summary>
public class MbsBscLgcGetManyRegionRspItemMdl
{
    /// <summary>管理者區域-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者區域-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者區域-是否啟用</summary>
    public bool ManagerRegionIsEnable { get; set; }
}