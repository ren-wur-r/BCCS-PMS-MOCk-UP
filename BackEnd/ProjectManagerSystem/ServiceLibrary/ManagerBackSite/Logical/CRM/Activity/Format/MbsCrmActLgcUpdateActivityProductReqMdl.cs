using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-更新活動產品-請求模型</summary>
public class MbsCrmActLgcUpdateActivityProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int? ManagerProductID { get; set; }
}
