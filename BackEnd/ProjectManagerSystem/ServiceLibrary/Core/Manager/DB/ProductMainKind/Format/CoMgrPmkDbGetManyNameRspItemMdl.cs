namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆[名稱]-項目-回應模型</summary>
public class CoMgrPmkDbGetManyNameRspItemMdl
{
    /// <summary>產品主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>產品主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }
}