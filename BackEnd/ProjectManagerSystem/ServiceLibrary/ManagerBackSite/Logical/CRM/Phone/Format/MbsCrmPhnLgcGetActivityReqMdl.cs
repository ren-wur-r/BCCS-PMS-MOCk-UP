using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動-請求模型</summary>
public class MbsCrmPhnLgcGetActivityReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }
}