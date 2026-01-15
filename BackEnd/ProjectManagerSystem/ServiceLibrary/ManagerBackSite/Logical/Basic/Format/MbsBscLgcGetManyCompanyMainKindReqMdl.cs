using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司主分類-請求模型</summary>
public class MbsBscLgcGetManyCompanyMainKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>公司主分類名稱(模糊查詢)</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}