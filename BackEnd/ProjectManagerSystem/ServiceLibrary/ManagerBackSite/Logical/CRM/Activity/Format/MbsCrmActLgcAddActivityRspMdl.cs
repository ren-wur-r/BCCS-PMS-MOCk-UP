using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增-回應模型</summary>
public class MbsCrmActLgcAddActivityRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }
}