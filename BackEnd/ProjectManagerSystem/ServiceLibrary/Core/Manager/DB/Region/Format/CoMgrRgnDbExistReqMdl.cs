namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-是否存在-請求模型</summary>
public class CoMgrRgnDbExistReqMdl
{
    /// <summary>管理者-地區-ID</summary>
    public int? ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

}