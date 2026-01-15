using DataModelLibrary.Database.AtomManagerContacter;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-新增-請求模型</summary>
public class CoMgrCttDbAddReqMdl
{
    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>管理者窗口-電話</summary>
    public string ManagerContacterPhone { get; set; }

    /// <summary>管理者窗口-部門</summary>
    public string ManagerContacterDepartment { get; set; }

    /// <summary>管理者窗口-職稱</summary>
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>管理者窗口-電話(市話)</summary>
    public string ManagerContacterTelephone { get; set; }

    /// <summary>管理者窗口-狀態</summary>
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>管理者窗口-是否個資同意</summary>
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>管理者窗口-開發評等</summary>
    public DbAtomManagerContacterRatingKindEnum ManagerContacterRatingKind { get; set; }

    /// <summary>管理者窗口-建立者員工ID</summary>
    public int ManagerContacterCreateEmployeeID { get; set; }

    /// <summary>管理者窗口-備註</summary>
    public string ManagerContacterRemark { get; set; }
}
