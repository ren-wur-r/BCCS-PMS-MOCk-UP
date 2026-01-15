using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司營業地點-請求模型</summary>
public class MbsBscLgcGetManyCompanyLocationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司營業地點名稱(模糊搜尋)</summary>
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}