namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得筆數從[管理者後台-系統-地區]-請求模型</summary>
public class CoMgrRgnDbGetCountFromMbsSysRgnReqMdl
{
    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    public bool? ManagerRegionIsEnable { get; set; }
}