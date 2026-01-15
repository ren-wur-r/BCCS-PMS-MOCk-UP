using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-刪除活動贊助-請求模型</summary>
public class MbsCrmActLgcRemoveActivitySponsorReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動贊助商ID</summary>
    public int ManagerActivitySponsorID { get; set; }
}
