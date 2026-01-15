using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得活動產品-回應模型</summary>
public class MbsCrmActLgcGetActivityProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-主分類-ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }
}
