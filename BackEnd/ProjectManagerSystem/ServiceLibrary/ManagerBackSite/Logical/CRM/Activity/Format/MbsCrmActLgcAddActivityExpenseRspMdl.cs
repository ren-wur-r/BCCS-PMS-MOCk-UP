using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動支出-回應模型</summary>
public class MbsCrmActLgcAddActivityExpenseRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動支出ID</summary>
    public int ManagerActivityExpenseID { get; set; }
}
