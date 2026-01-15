using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-新增商機產品-回應模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機產品ID</summary>
    public int EmployeePipelineProductID { get; set; }
}