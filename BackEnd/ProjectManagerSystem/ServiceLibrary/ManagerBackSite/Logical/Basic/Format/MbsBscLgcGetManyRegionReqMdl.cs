using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者區域-取得多筆-請求模型</summary>
public class MbsBscLgcGetManyRegionReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>區域-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>是否啟用</summary>
    public bool? ManagerRegionIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}