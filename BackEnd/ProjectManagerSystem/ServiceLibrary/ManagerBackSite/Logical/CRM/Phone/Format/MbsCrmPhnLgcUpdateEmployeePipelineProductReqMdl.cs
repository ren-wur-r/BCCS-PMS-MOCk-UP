using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新電銷產品-請求模型</summary>
public class MbsCrmPhnLgcUpdateEmployeePipelineProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>電銷產品ID</summary>
    public int EmployeePipelineProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int? ManagerProductID { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品子分類ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int? ManagerProductSpecificationID { get; set; }
}