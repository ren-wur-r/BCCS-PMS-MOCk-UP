namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-新增-請求模型</summary>
public class CoMgrRgnDbAddReqMdl
{
    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    public bool ManagerRegionIsEnable { get; set; }
}
