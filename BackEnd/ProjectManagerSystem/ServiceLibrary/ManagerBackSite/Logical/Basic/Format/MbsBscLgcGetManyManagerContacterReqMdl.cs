using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆窗口-請求模型</summary>
public class MbsBscLgcGetManyManagerContacterReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口-名稱(模糊查詢)</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-電子郵件(模糊查詢)</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}
