using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案-請求模型</summary>
public class MbsBscLgcGetManyProjectReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案-名稱(模糊查詢)</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}