using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動產品-回應模型</summary>
public class MbsCrmActLgcAddActivityProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }
}
