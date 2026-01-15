using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

/// <summary>管理者後台-系統設定-地區-取得多筆-項目-請求模型</summary>
public class MbsSysRgnLgcGetManyItemReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    public bool? ManagerRegionIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; } = 10;
}