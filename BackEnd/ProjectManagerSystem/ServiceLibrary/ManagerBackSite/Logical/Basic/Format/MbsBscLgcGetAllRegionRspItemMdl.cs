namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得全部管理者地區-回應項目模型</summary>
public class MbsBscLgcGetAllRegionRspItemMdl
{
    /// <summary>管理者地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者地區-名稱</summary>
    public string ManagerRegionName { get; set; }
}