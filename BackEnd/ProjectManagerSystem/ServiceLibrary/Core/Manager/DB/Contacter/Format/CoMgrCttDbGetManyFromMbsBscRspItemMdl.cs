namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-基本]-回應項目</summary>
public class CoMgrCttDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口-名稱</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-電子郵件</summary>
    public string ManagerContacterEmail { get; set; }
}