namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得-回應模型</summary>
public class CoMgrRgnDbGetRspMdl
{
    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    public bool ManagerRegionIsEnable { get; set; }
}