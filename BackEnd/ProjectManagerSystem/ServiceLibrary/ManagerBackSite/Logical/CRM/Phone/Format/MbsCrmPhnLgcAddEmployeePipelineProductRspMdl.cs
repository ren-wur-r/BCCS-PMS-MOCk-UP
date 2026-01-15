using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增商機產品-回應模型</summary>
public class MbsCrmPhnLgcAddEmployeePipelineProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機產品ID</summary>
    public int EmployeePipelineProductID { get; set; }
}