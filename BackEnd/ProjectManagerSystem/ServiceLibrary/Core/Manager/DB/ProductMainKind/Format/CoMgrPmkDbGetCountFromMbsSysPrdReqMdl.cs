namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得筆數從[管理者後台-系統設定-產品[主分類]]-回應模型</summary>
public class CoMgrPmkDbGetCountFromMbsSysPrdReqMdl
{
    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    public bool? ManagerProductMainKindIsEnable { get; set; }
}