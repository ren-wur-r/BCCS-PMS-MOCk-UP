using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司子分類-請求模型</summary>
public class MbsBscLgcGetManyCompanySubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>公司主分類ID</summary>
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>公司子分類名稱(模糊查詢)</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>公司子分類是否啟用</summary>
    public bool? ManagerCompanySubKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}
