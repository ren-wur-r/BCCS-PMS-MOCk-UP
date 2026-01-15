using DataModelLibrary.Database.AtomPipeline;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動名單-請求模型</summary>
public class MbsCrmActLgcAddActivityEmployeePipelineReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>客戶公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>客戶公司營業地點ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }
}
