using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-刪除活動支出-請求模型</summary>
public class MbsCrmActLgcRemoveActivityExpenseReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動支出ID</summary>
    public int ManagerActivityExpenseID { get; set; }
}
