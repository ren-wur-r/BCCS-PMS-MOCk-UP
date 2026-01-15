using DataModelLibrary.Database.AtomEmployeePipelineBill;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機-請求模型</summary>
public class MbsCrmPplLgcUpdateEmployeePipelineReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機發票紀錄ID</summary>
    public int EmployeePipelineBillID { get; set; }

    /// <summary>發票號碼</summary>
    public string EmployeePipelineBillNumber { get; set; }

    /// <summary>發票狀態</summary>
    public DbAtomEmployeePipelineBillStatusEnum? EmployeePipelineBillStatus { get; set; }

    /// <summary>備註</summary>
    public string EmployeePipelineBillRemark { get; set; }
}