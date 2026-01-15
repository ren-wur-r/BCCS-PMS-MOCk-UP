namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得筆數從[管理者後台-系統設定-產品]-回應模型</summary>
public class CoMgrPrdDbGetCountFromMbsSysPrdReqMdl
{
    /// <summary>管理者-產品-主分類ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-是否為主力產品</summary>
    public bool? ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

}