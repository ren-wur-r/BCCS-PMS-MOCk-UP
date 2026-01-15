using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆專案里程碑-請求模型</summary>
public class MbsBscLgcGetManyProjectStoneReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

}