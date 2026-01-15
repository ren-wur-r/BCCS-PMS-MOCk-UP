namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆名稱-回應項目模型</summary>
public class CoMgrCttDbGetManyNameRspItemMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    public string ManagerContacterName { get; set; }
}
