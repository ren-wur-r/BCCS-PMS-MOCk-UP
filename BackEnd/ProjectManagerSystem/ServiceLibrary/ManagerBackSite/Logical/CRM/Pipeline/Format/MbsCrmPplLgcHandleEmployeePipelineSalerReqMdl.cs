using DataModelLibrary.Database.AtomEmployeePipelineSaler;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-處理商機業務-請求模型</summary>
public class MbsCrmPplLgcHandleEmployeePipelineSalerReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-備注</summary>
    public string EmployeePipelineSalerRemark { get; set; }

    /// <summary>商機業務-轉指派業務員工ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }
}
