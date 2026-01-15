using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-刪除商機產品-請求模型</summary>
public class MbsCrmPplLgcRemoveEmployeePipelineProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機產品ID</summary>
    public int EmployeePipelineProductID { get; set; }
}