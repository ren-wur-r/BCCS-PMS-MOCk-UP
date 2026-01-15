using DataModelLibrary.Database.AtomManagerContacter;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-客戶名單-客戶窗口]-回應項目模型</summary>
public class CoMgrCttDbGetManyFrommbsSysCtrRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }

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

    /// <summary>管理者窗口-開發評等</summary>
    public DbAtomManagerContacterRatingKindEnum ManagerContacterRatingKind { get; set; }
}
