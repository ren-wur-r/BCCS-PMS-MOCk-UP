using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得電銷產品-回應模型</summary>
public class MbsCrmPhnLgcGetEmployeePipelineProductRspMdl : MbsLgcBaseRspMdl
{

    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者產品主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品子分類名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者產品規格名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

}