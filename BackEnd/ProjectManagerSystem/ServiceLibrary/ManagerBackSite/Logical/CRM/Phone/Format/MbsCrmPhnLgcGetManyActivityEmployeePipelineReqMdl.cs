using DataModelLibrary.Database.AtomPipeline;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單-請求模型</summary>
public class MbsCrmPhnLgcGetManyActivityEmployeePipelineReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum? AtomPipelineStatus { get; set; }

    /// <summary>管理者公司名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>窗口姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}