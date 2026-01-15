namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoMgrRgnDbGetManyFromMbsBscReqMdl
{
    /// <summary>地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>地區-是否啟用</summary>
    public bool? ManagerRegionIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}