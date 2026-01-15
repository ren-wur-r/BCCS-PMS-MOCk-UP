using DataModelLibrary.Database.AtomManagerContacter;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得原始窗口-請求模型</summary>
public class MbsCrmPhnLgcGetOriginalEmployeePipelineContacterRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>窗口姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口部門</summary>
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口電話(市話)</summary>
    public string ManagerContacterTelephone { get; set; }

    /// <summary>窗口是否個資同意</summary>
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>窗口在職狀態</summary>
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>窗口開發評等ID</summary>
    public DbAtomManagerContacterRatingKindEnum AtomRatingKind { get; set; }
}