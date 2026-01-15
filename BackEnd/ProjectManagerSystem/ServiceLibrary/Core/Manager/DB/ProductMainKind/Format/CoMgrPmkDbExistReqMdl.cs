namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-是否存在-請求模型</summary>
public class CoMgrPmkDbExistReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }
}