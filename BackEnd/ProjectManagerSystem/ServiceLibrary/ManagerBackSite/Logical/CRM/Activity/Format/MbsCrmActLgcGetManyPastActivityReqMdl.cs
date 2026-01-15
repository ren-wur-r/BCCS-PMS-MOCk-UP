using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆客戶過往活動-請求模型</summary>
public class MbsCrmActLgcGetManyPastActivityReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}