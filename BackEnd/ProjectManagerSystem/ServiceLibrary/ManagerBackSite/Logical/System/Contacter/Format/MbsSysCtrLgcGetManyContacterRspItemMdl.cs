using DataModelLibrary.Database.AtomManagerContacter;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得多筆窗口-回應項目模型</summary>
public class MbsSysCtrLgcGetManyContacterRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>管理者公司名稱</summary>
    public string ManagerCompanyName { get; set; }

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
