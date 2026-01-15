using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-更新名單客戶-請求模型</summary>
public class MbsCrmPplLgcUpdateEmployeePipelineCompanyReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>客戶公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>客戶公司營業地點ID</summary>
    public int ManagerCompanyLocationID { get; set; }
}